using System;
using System.Collections.Generic;
using RestSharp;

namespace Pitter
{
    internal static class Scavenger
    {
        public static bool TryFindUser(this string screenName, out User user)
        {
            throw new NotImplementedException();
        }

        public static bool TryGetFriends(this string screenName, out IEnumerable<User> friends)
        {
            throw new NotImplementedException();
        }
    }
}