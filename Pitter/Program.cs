using System;
using System.Linq;

namespace Pitter
{
    internal class Program
    {
        private const string LinkKeyword = "shorten";
        private const string ExistKeyword = "find";

        private static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine($@"
Привет!
Я могу сделать вот что:
    + {ExistKeyword}            Поискать пользователя по screen_name                (это та часть, что после vk.com/)
    + {LinkKeyword}      Могу показать список друзей пользователя            (Покажу ФИО и screen_name)
");
                    return;
                }

                if (args.Length == 1)
                {
                    Console.WriteLine("Укажи пожалуйста параметры");
                    return;
                }

                if (args[0] == ExistKeyword)
                {
                    Console.WriteLine("Ищу пользователя...");
                    Console.WriteLine(args[1].TryFindUser(out var user)
                                          ? $"Да, такой пользователь есть, вот: {user}"
                                          : "Такого пользователя нет :(");
                    return;
                }

                if (args[0] != LinkKeyword)
                    return;
                {
                    Console.WriteLine("Сокращаю ссылку...");
                    if (args[1].TryShortenLink(out var link))
                        Console.WriteLine("А вот и ссылка:\n" +
                                          $"{link.Response}");
                    else
                        Console.WriteLine("Ссылка какая-то не такая :(");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Что-то пошло не так :(");
            }
        }
    }
}