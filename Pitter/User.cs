using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Pitter {
    public class User
    {
        [JsonExtensionData] public IDictionary<string, JToken> ExtraStuff = new Dictionary<string, JToken>();

        public string FirstName { get; set; }

        public bool CanAccessClosed { get; set; }
        public int Sex { get; set; }
        public bool Verified { get; set; }
        public bool IsClosed { get; set; }

        public long Id { get; set; }

        public string LastName { get; set; }

        public string Bdate { get; set; }

        public string ScreenName { get; }

        public CityInfo City { get; set; }

        private string PrintIfNotNull<T>(T item, string name)
        {
            if (item == null)
                return "";
            return $"{name}: {item},\n";
        }

        public override string ToString()
        {
            var s =
                $"{PrintIfNotNull(LastName, "Фамилия")}" +
                $"{PrintIfNotNull(FirstName, "Имя")}" +
                $"{PrintIfNotNull(Id, nameof(Id))}" +
                $"{PrintIfNotNull(Bdate, "День рождения")}" +
                $"Пол: {(Sex == 2 ? "Мужской" : "Женский")},\n" +
                $"Verified: {(Verified ? "Да" : "Нет")},\n" +
                $"Профиль закрыт: {(IsClosed ? "Да" : "Нет")},\n" +
                $"{PrintIfNotNull(ScreenName, nameof(ScreenName))}" +
                $"{PrintIfNotNull(City, "Город")}";
            return "\n" + s + (ExtraStuff.Count > 0
                                   ? $"And some additional unrecognized data:{string.Join("\n", ExtraStuff.Select(kv => $"\t{kv.Key}: {kv.Value}"))}"
                                   : "");
        }

        public class CityInfo
        {
            public long Id { get; set; }
            public string Title { get; set; }

            public override string ToString() => $"\t{nameof(Id)}: {Id}," +
                                                 $"\t{nameof(Title)}: {Title}";
        }
    }
}