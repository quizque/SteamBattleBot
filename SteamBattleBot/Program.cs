using System;
using SteamKit2;
using System.IO;
using System.Threading;

namespace SteamBot
{
    class Program
    {
        static SteamClient steamClient;
        static CallbackManager manager;
        static SteamUser steamUser;
        static SteamFriends steamFriends;

        static bool isRunning, isPlaying;

        static string user, pass;

        static string authCode;

        static Random _random = new Random();

        static int monsterHP;
        static int playerHP;
        static int dmgDonePlayer;
        static int dmgDoneMonster;


        static void Main(string[] args)
        {
            SteamDirectory.Initialize();
            if (!File.Exists("chat.txt"))
            {
                File.Create("chat.txt").Close();
                File.WriteAllText("chat.txt", "abc | 123");
            }
            if (!File.Exists("admin.txt"))
            {
                File.Create("admin.txt").Close();
                File.WriteAllText("admin.txt", "76561198116385237");
            }
            Console.Title = "Steam Battle Bot";
            StartUpLogo();
            Console.WriteLine("Press CTRL+C to quit or send !shutdown as admin to bot");

            Console.Write("Username: ");
            user = Console.ReadLine();
            Console.Write("Password: ");
            pass = inputPass();

            SteamLogIn();
        }

        static void SteamLogIn()
        {
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();

            steamFriends = steamClient.GetHandler<SteamFriends>();

            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);

            manager.Subscribe<SteamUser.AccountInfoCallback>(OnAccountInfo);
            manager.Subscribe<SteamFriends.FriendMsgCallback>(OnChatMessage);

            manager.Subscribe<SteamFriends.FriendsListCallback>(OnFriendsAdded);

            manager.Subscribe<SteamUser.UpdateMachineAuthCallback>(OnMachineAuth);

            isRunning = true;

            Console.WriteLine("Connecting to Steam...");

            steamClient.Connect();

            while (isRunning)
            {
                manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }
            Console.ReadKey();
        }

        static void OnConnected(SteamClient.ConnectedCallback callBack)
        {
            if (callBack.Result != EResult.OK)
            {
                Console.WriteLine("Unable to connect to Steam: {0}", callBack.Result);
                isRunning = false;
                return;
            }

            Console.WriteLine("Connected to Steam! Logging in {0}...", user);

            byte[] sentryHash = null;

            if (File.Exists("sentry.bin"))
            {
                byte[] sentryFile = File.ReadAllBytes("sentry.bin");
                sentryHash = CryptoHelper.SHAHash(sentryFile);
            }

            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = user,
                Password = pass,
                AuthCode = authCode,
                SentryFileHash = sentryHash,
            });
        }

        static void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Console.WriteLine("{0} disconnected from Steam, reconnecting in 5...", user);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            steamClient.Connect();
        }

        static void OnLoggedOn(SteamUser.LoggedOnCallback callBack)
        {
            if (callBack.Result != EResult.OK)
            {
                if (callBack.Result == EResult.AccountLogonDenied)
                {
                    Console.WriteLine("Unable to logon to Steam: This account is SteamGuard protected.");

                    Console.Write("Please enter the auth code sent to the email at {0}: ", callBack.EmailDomain);
                    authCode = Console.ReadLine();
                    return;
                }

                Console.WriteLine("Unable to logon to Steam: {0} / {1}", callBack.Result, callBack.ExtendedResult);
                isRunning = false;
                return;
            }
            Console.WriteLine("{0} succesfully logged in!", user);
        }

        static void OnLoggedOff(SteamUser.LoggedOffCallback callBack)
        {
            Console.WriteLine("Logged off of Steam: {0}", callBack.Result);
        }

        static void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callBack)
        {
            Console.WriteLine("Updating sentry file...");
            byte[] sentryHash = CryptoHelper.SHAHash(callBack.Data);
            File.WriteAllBytes("sentry.bin", callBack.Data);

            steamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails
            {
                JobID = callBack.JobID,
                FileName = callBack.FileName,
                BytesWritten = callBack.BytesToWrite,
                FileSize = callBack.Data.Length,
                Offset = callBack.Offset,
                Result = EResult.OK,
                LastError = 0,
                OneTimePassword = callBack.OneTimePassword,
                SentryFileHash = sentryHash,
            });

            Console.WriteLine("Done.");
        }

        static void OnAccountInfo(SteamUser.AccountInfoCallback callBack)
        {
            steamFriends.SetPersonaState(EPersonaState.Online);
        }

        static void OnChatMessage(SteamFriends.FriendMsgCallback callBack)
        {
            //string[] args; UNCOMMECT WHEN BETTER SETUP IS ADDED!
            if (callBack.EntryType == EChatEntryType.ChatMsg)
            {
                if (callBack.Message.Length > 1)
                {
                    if (callBack.Message.Remove(1) == "!")
                    {
                        string command = callBack.Message;
                        if (callBack.Message.Contains(" "))
                        {
                            command = callBack.Message.Remove(callBack.Message.IndexOf(" "));
                        }

                        switch (command)
                        {
                            case "!shutdown":
                                if (!isBotAdmin(callBack.Sender))
                                {
                                    steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Only admins can use the !shutdown command!");
                                    break;
                                }
                                Environment.Exit(0);
                                break;

                            case "!setup":
                                if (!isBotAdmin(callBack.Sender))
                                {
                                    steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Only admins can use the !setup command!");
                                    break;
                                }
                                if (!isPlaying)
                                {
                                    steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "A game is already setup!");
                                    steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Resetting...");
                                }
                                Console.WriteLine("!setup command recived. User: " + steamFriends.GetFriendPersonaName(callBack.Sender));
                                steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Setting up game...");
                                monsterHP = _random.Next(50, 150);
                                playerHP = 100;
                                isPlaying = true;
                                steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Done setting up!");
                                steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Ready to play!");
                                break;

                            case "!attack":
                                if (!isPlaying)
                                {
                                    steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "There is no currect game!");
                                    break;
                                }
                                Console.WriteLine("!attack command recived. User: " + steamFriends.GetFriendPersonaName(callBack.Sender));
                                dmgDonePlayer = _random.Next(5, 25);
                                dmgDoneMonster = _random.Next(5, 15);
                                steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "You did " + dmgDonePlayer + " to the monster!");
                                monsterHP -= dmgDonePlayer;
                                steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "The monster did " + dmgDoneMonster + " to you!");
                                playerHP -= dmgDoneMonster;
                                postState(callBack.Sender);
                                break;

                            case "!state":
                                if (!isPlaying)
                                {
                                    steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "There is no currect game!");
                                    break;
                                }
                                Console.WriteLine("!state command recived. User: " + steamFriends.GetFriendPersonaName(callBack.Sender));
                                postState(callBack.Sender);
                                break;
                        }
                    }
                }
                string rLine;
                string trimmed = callBack.Message;
                char[] trim = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '[', ']', '{', '}', '\\', '|', ';', ':', '"', '\'', ',', '<', '.', '>', '/', '?' };

                for (int i = 0; i < 30; i++)
                {
                    trimmed = trimmed.Replace(trim[i].ToString(), "");
                }

                StreamReader sReader = new StreamReader("chat.txt");

                while ((rLine = sReader.ReadLine()) != null)
                {
                    string text = rLine.Remove(rLine.IndexOf('|') - 1);
                    string response = rLine.Remove(0, rLine.IndexOf('|') + 2);

                    if (callBack.Message.ToLower().Contains(text))
                    {
                        steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, response);
                        sReader.Close();
                        return;
                    }
                }
            }
        }

        public static void postState(SteamID sid)
        {
            Console.WriteLine("Currect State");
            steamFriends.SendChatMessage(sid, EChatEntryType.ChatMsg, "Currect state.");
            Console.WriteLine("Player HP: " + playerHP);
            steamFriends.SendChatMessage(sid, EChatEntryType.ChatMsg, "Your HP: " + playerHP);
            Console.WriteLine("Monster HP: " + monsterHP);
            steamFriends.SendChatMessage(sid, EChatEntryType.ChatMsg, "Monsters HP: " + monsterHP);
        }

        public static bool isBotAdmin(SteamID sid)
        {
            try
            {
                if (sid.ConvertToUInt64() == Convert.ToUInt64(File.ReadAllText("admin.txt")))
                {
                    return true;
                }
                steamFriends.SendChatMessage(sid, EChatEntryType.ChatMsg, "You are not a bot admin.");
                Console.WriteLine(steamFriends.GetFriendPersonaName(sid) + " attempted to use an administrator command while not an administrator.");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        static void OnFriendsAdded(SteamFriends.FriendsListCallback callBack)
        {
            Thread.Sleep(2500);

            foreach (var friend in callBack.FriendList)
            {
                if (friend.Relationship == EFriendRelationship.RequestRecipient)
                {
                    steamFriends.AddFriend(friend.SteamID);
                    Thread.Sleep(500);
                    steamFriends.SendChatMessage(friend.SteamID, EChatEntryType.ChatMsg, "Hello, I am Nicks battle bot. Ask nickthegamer5 to start a battle!");
                }
            }
        }

        public static string[] seperate(int number, char seperator, string thestring)
        {
            string[] returned = new string[4];

            int i = 0;
            int error = 0;
            int length = thestring.Length;

            foreach (char c in thestring)
            {
                if (i != number)
                {
                    if (error > length || number > 5)
                    {
                        returned[0] = "-1";
                        return returned;
                    }
                    else if (c == seperator)
                    {
                        returned[i] = thestring.Remove(thestring.IndexOf(c));
                        thestring = thestring.Remove(0, thestring.IndexOf(c) + 1);
                        i++;
                    }
                    error++;

                    if (error == length && i != number)
                    {
                        returned[0] = "-1";
                        return returned;
                    }
                }
                else
                {
                    returned[i] = thestring;
                }
            }
            return returned;
        }

        static string inputPass()
        {
            {
                string pass = "";
                bool protectPass = true;
                while (protectPass)
                {
                    char s = Console.ReadKey(true).KeyChar;
                    if (s == '\r')
                    {
                        protectPass = false;
                        Console.WriteLine();
                    }
                    else if (s == '\b' && pass.Length > 0)
                    {
                        Console.CursorLeft -= 1;
                        Console.Write(' ');
                        Console.CursorLeft -= 1;
                        pass = pass.Substring(0, pass.Length - 1);
                    }
                    else
                    {
                        pass = pass + s.ToString();
                        Console.Write("*");
                    }

                }
                return pass;
            }
        }

        static void StartUpLogo()
        {

            Console.WriteLine(@" _____ _                        ______       _    ______       _   _   _      ");
            Console.WriteLine(@"/  ___| |                       | ___ \     | |   | ___ \     | | | | | |     ");
            Console.WriteLine(@"\ `--.| |_ ___  __ _ _ __ ___   | |_/ / ___ | |_  | |_/ / __ _| |_| |_| | ___ ");
            Console.WriteLine(@" `--. | __/ _ \/ _` | '_ ` _ \  | ___ \/ _ \| __| | ___ \/ _` | __| __| |/ _ \");
            Console.WriteLine(@"/\__/ | ||  __| (_| | | | | | | | |_/ | (_) | |_  | |_/ | (_| | |_| |_| |  __/");
            Console.WriteLine(@"\____/ \__\___|\__,_|_| |_| |_| \____/ \___/ \__| \____/ \__,_|\__|\__|_|\___|");
        }
    }
}