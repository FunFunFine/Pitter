using System;
using System.Linq;

namespace Pitter
{
    class Program
    {
        const string FriendsKeyword = "getFriends";
        const string ExistKeyword = "find";

        static void Main(string[] args)
        {
            if (args.Length==0)
            {
                Console.WriteLine($@"
Привет!
Я могу сделать вот что:
    + {ExistKeyword}            Поискать пользователя по screen_name                (это та часть, что после vk.com/)
    + {FriendsKeyword}      Могу показать список друзей пользователя            (Покажу ФИО и screen_name)
");
                return;
            }
            if (args.Length == 1)
            {
                Console.WriteLine("Укажи пожалуйста screen_name");
                return;
            }
            if (args[0] == ExistKeyword)
            {
                Console.WriteLine(args[1].TryFindUser(out var user)
                    ? $"Да, такой пользователь есть, вот: {user}"
                    : "Такого пользователя нет :(");
                return;
            }

            if (args[0] != FriendsKeyword)
                return;
            {
                if (args[1].TryGetFriends(out var friends))
                    Console.WriteLine("А вот и друзья этого пользователя:" +
                                      $"{friends.Aggregate("", (acc, user) => $"{acc}\n\t{user}")}");
                else
                {
                    Console.WriteLine("Такого пользователя нет :(");
                }
            }
        }
    }

    public class User
    {
        public User(string screenName)
        {
            ScreenName = screenName;
        }

        public string ScreenName { get; }
    }
}
