using System;
using SteamKit2;
using System.IO;

namespace SteamBattleBot
{
    class Program
    {
        static SteamFunctions steam = new SteamFunctions();

        static void Main(string[] args)
        {
            bool autologin = true;

            SteamDirectory.Initialize();
            if (!File.Exists("admin.txt"))
            {
                File.Create("admin.txt").Close();
                File.WriteAllText("admin.txt", "76561198116385237");
            }
            if (!File.Exists("creds.txt") && autologin)
            {
                File.Create("creds.txt").Close();
            }

            Console.Title = "Steam Battle Bot";

            Console.WriteLine(@" _____ _                        ______       _    ______       _   _   _      ");
            Console.WriteLine(@"/  ___| |                       | ___ \     | |   | ___ \     | | | | | |     ");
            Console.WriteLine(@"\ `--.| |_ ___  __ _ _ __ ___   | |_/ / ___ | |_  | |_/ / __ _| |_| |_| | ___ ");
            Console.WriteLine(@" `--. | __/ _ \/ _` | '_ ` _ \  | ___ \/ _ \| __| | ___ \/ _` | __| __| |/ _ \");
            Console.WriteLine(@"/\__/ | ||  __| (_| | | | | | | | |_/ | (_) | |_  | |_/ | (_| | |_| |_| |  __/");
            Console.WriteLine(@"\____/ \__\___|\__,_|_| |_| |_| \____/ \___/ \__| \____/ \__,_|\__|\__|_|\___|");

            Console.WriteLine("Press CTRL+C to quit or send !shutdown as admin to bot");

            if (File.Exists("creds.txt") && autologin)
            {
                if (new FileInfo("creds.txt").Length != 0) {
                    Console.WriteLine("Autologin file detected!");
                    string[] lines = File.ReadAllLines("creds.txt");
                    steam.user = lines[0];
                    steam.pass = lines[1];
                }
                else
                {
                    Console.WriteLine("creds.txt is empty! Exiting...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }else
            {
                Console.Write("Username: ");
                steam.user = Console.ReadLine();
                Console.Write("Password: ");
                steam.pass = steam.inputPass();
            }

            if (steam.user == "" || steam.pass == "")
            {
                Console.WriteLine("Your username/password is blank!");
                Console.ReadKey();
                Environment.Exit(0);
            }

            steam.SteamLogIn();
        }
    }
}