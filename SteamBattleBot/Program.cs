using System;
using SteamKit2;
using System.IO;

namespace SteamBattleBot
{
    class Program
    {
        static SteamFunctions steam = new SteamFunctions(); // Make a new SteamFunctions class

        static void Main(string[] args)
        {
            SteamDirectory.Initialize(); // Initialize the Steam Directory. Removing this will break the program.
            steam.Log("|");
            // Setup console display
            #region Console Display Setup
            Console.Title = "Steam Battle Bot";
            Console.WriteLine(@" _____ _                        ______       _    ______       _   _   _      " + Environment.NewLine
                            + @"/  ___| |                       | ___ \     | |   | ___ \     | | | | | |     " + Environment.NewLine
                            + @"\ `--.| |_ ___  __ _ _ __ ___   | |_/ / ___ | |_  | |_/ / __ _| |_| |_| | ___ " + Environment.NewLine
                            + @" `--. | __/ _ \/ _` | '_ ` _ \  | ___ \/ _ \| __| | ___ \/ _` | __| __| |/ _ \" + Environment.NewLine
                            + @"/\__/ | ||  __| (_| | | | | | | | |_/ | (_) | |_  | |_/ | (_| | |_| |_| |  __/" + Environment.NewLine
                            + @"\____/ \__\___|\__,_|_| |_| |_| \____/ \___/ \__| \____/ \__,_|\__|\__|_|\___|");
            Console.WriteLine("Press CTRL+C to quit or send !shutdown as admin to bot");
            #endregion

            // Checking files and console input
            #region Check for admin.txt
            if (!File.Exists("admin.txt"))
            {
                File.Create("admin.txt").Close();
                File.WriteAllText("admin.txt", "76561198116385237");
            }
            #endregion

            #region Check for creds.txt
            if (File.Exists("creds.txt"))
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
                steam.pass = steam.InputPass();
            }
            #endregion

            #region Check to see if user/pass is blank
            if (steam.user == "" || steam.pass == "")
            {
                Console.WriteLine("Your username/password is blank!");
                Console.ReadKey();
                Environment.Exit(0);
            }
            #endregion

            steam.SteamLogIn(); // Start the main program
        }
    }
}