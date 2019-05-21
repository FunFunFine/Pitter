using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace Pitter
{
    internal static class Scavenger
    {
        private const string VkTokenVariableName = "VK_TOKEN";
        private const string VkUri = "https://api.vk.com/method";
        private static readonly RestClient RestClient = new RestClient(VkUri);

        public static string Token => Environment.GetEnvironmentVariable(VkTokenVariableName) ??
                                      "02bb1c4175ce5277bac6ab6eb226a7aec5df61a110dd9d34a6f0ce4cfc68753472b98ac38e48586c61840";

        public static bool TryFindUser(this string screenName, out User user)
        {
            user = null;

            if (!ExecuteResolveScreenNameRequest(screenName)
                 .Content.TryParseJson(out ResponseContainer<ScreenNameAnswer> answer))
                return false;
            var content = ExecuteGetUserRequest(answer.Response.ObjectId).Content;

            content.TryParseJson(out ResponseContainer<User[]> userResponse);
            if (userResponse?.Response?[0] == null)
                return false;
            user = userResponse.Response[0];
            return true;
        }

        public static bool TryParseJson<T>(this string @this, out T result)
        {
            var success = true;
            var settings = new JsonSerializerSettings
                           {
                               Error = (sender, args) =>
                               {
                                   success = false;
                                   args.ErrorContext.Handled = true;
                               },
                               MissingMemberHandling = MissingMemberHandling.Error,
                               ContractResolver = new DefaultContractResolver
                                                  {
                                                      NamingStrategy = new SnakeCaseNamingStrategy()
                                                  },
                               Formatting = Formatting.Indented
                           };
            result = JsonConvert.DeserializeObject<T>(@this, settings);
            return success;
        }

        private static IRestResponse ExecuteResolveScreenNameRequest(string screenName)
        {
            var request = new RestRequest("utils.resolveScreenName", Method.GET)
                          {
                              OnBeforeDeserialization = resp => resp.ContentType = "application/json"
                          };
            request.AddParameter("v", "5.92");
            request.AddParameter("access_token", Token);
            request.AddParameter("screen_name", screenName);
            return RestClient.Execute(request);
        }

        private static IRestResponse ExecuteGetUserRequest(long userId)
        {
            var request = new RestRequest("users.get", Method.GET)
                          {
                              OnBeforeDeserialization = resp => resp.ContentType = "application/json"
                          };
            request.AddParameter("v", "5.92");
            request.AddParameter("access_token", Token);
            request.AddParameter("user_ids", userId);
            request.AddParameter("fields",
                                 "id, first_name, last_name, bdate, about, city, education, relation, sex, verified");
            return RestClient.Execute(request);
        }

        private static IRestResponse SendRequest(string method, params Parameter[] parameters)
        {
            IRestRequest request = new RestRequest(method, Method.GET)
                                   {
                                       OnBeforeDeserialization = resp => resp.ContentType = "application/json"
                                   };
            return RestClient.Execute(parameters
                                      .Concat(new[]
                                              {
                                                  new Parameter("v", "5.92", ParameterType.GetOrPost),
                                                  new Parameter("access_token", Token, ParameterType.GetOrPost)
                                              })
                                      .Aggregate(request, (acc, p) => acc.AddParameter(p)));
        }

        public static bool TryShortenLink(this string fullLink, out ResponseContainer<ShortenedLink> link)
        {
            var request = new RestRequest("utils.getShortLink", Method.GET)
                          {
                              OnBeforeDeserialization = resp => resp.ContentType = "application/json"
                          };
            request.AddParameter("v", "5.92");
            request.AddParameter("access_token", Token);
            request.AddParameter("url", fullLink);
            request.AddParameter("private", 0);
            var response = RestClient.Execute(request);
            return response.Content.TryParseJson(out link);
        }
    }
}