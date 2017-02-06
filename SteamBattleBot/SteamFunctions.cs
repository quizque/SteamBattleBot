using System;
using SteamKit2;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace SteamBattleBot
{
    class SteamFunctions
    {
        SteamClient steamClient;
        CallbackManager manager;
        SteamUser steamUser;
        SteamFriends steamFriends;

        bool isRunning;

        public string user, pass;

        string[] lines;

        string authCode, twoFactorAuth;
        byte[] sentryHash;

        int playerCount = 0;

        Random _random = new Random();

        List<Structures.PlayerStructure> players = new List<Structures.PlayerStructure>();
        List<ulong> admins = new List<ulong>();

        public void SteamLogIn()
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

            try
            {
                lines = File.ReadAllLines("admin.txt"); // Read all the SteamID64s in the file
                admins.Clear(); // Clear the list to store the new admins

                for (int i = 0; i<lines.Length; i++) // Read each line in var
                {
                    try // Try to convert it
                    {
                        admins.Add(Convert.ToUInt64(lines[i])); // Add ID to list
                    }
                    catch // If invalid...
                    {
                        Console.WriteLine("WARNING: {0} is an invalid steamID16 in admin.txt! Skipping...", lines[i]); // Tell the user
                    }
                }
            }catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            players.Add(new Structures.PlayerStructure { id = 00000000000000000 });

            Console.WriteLine("Connecting to Steam...");

            steamClient.Connect();

            while (isRunning)
            {
                manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }
            Console.ReadKey();
        }

        void OnConnected(SteamClient.ConnectedCallback callBack)
        {

            if (callBack.Result != EResult.OK)
            {
                Console.WriteLine("Unable to connect to Steam: {0}", callBack.Result);
                isRunning = false;
                return;
            }

            Console.WriteLine("Connected to Steam! Logging in {0}...", user);

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
                TwoFactorCode = twoFactorAuth,
                SentryFileHash = sentryHash,
            });
        }

        void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            Console.WriteLine("{0} disconnected from Steam, reconnecting in 5...", user);

            Thread.Sleep(TimeSpan.FromSeconds(5));

            steamClient.Connect();
        }

        void OnLoggedOn(SteamUser.LoggedOnCallback callBack)
        {
            bool isSteamGuard = callBack.Result == EResult.AccountLogonDenied;
            bool is2FA = callBack.Result == EResult.AccountLoginDeniedNeedTwoFactor;
            if (isSteamGuard || is2FA)
            {
                Console.WriteLine("This account is SteamGuard protected!");

                if (is2FA)
                {
                    Console.Write("Please enter your 2 factor auth code from your authenticator app: ");
                    twoFactorAuth = Console.ReadLine();
                }
                else
                {
                    Console.Write("Please enter the auth code sent to the email at {0}: ", callBack.EmailDomain);
                    authCode = Console.ReadLine();
                }
                return;
            }
            if (callBack.Result != EResult.OK)
            {
                Console.WriteLine("Unable to logon to Steam: {0} / {1}", callBack.Result, callBack.ExtendedResult);

                isRunning = false;
                return;
            }

            Console.WriteLine("Successfully logged on!");
        }

        void OnLoggedOff(SteamUser.LoggedOffCallback callBack)
        {
            Console.WriteLine("Logged off of Steam: {0}", callBack.Result);
        }

        void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callBack)
        {
            Console.WriteLine("Updating sentry file...");
            int fileSize;
            byte[] sentryHash;

            using (var fs = File.Open("sentry.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fs.Seek(callBack.Offset, SeekOrigin.Begin);
                fs.Write(callBack.Data, 0, callBack.BytesToWrite);
                fileSize = (int)fs.Length;

                fs.Seek(0, SeekOrigin.Begin);
                using (var sha = new SHA1CryptoServiceProvider())
                {
                    sentryHash = sha.ComputeHash(fs);
                }
            }

            steamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails
            {
                JobID = callBack.JobID,

                FileName = callBack.FileName,

                BytesWritten = callBack.BytesToWrite,
                FileSize = fileSize,
                Offset = callBack.Offset,

                Result = EResult.OK,
                LastError = 0,

                OneTimePassword = callBack.OneTimePassword,

                SentryFileHash = sentryHash,
            });

            Console.WriteLine("Done.");
        }

        void OnAccountInfo(SteamUser.AccountInfoCallback callBack)
        {
            steamFriends.SetPersonaState(EPersonaState.Online);
        }

        void OnChatMessage(SteamFriends.FriendMsgCallback callBack)
        {
            //string[] args; UNCOMMECT WHEN BETTER SETUP IS ADDED!
            if (callBack.EntryType == EChatEntryType.ChatMsg)
            {
                if (callBack.Message.Length > 1 && callBack.Message.Remove(1) == "!")
                {
                    string command = callBack.Message;
                    if (callBack.Message.Contains(" "))
                    {
                        command = callBack.Message.Remove(callBack.Message.IndexOf(" "));
                    }

                    switch (command)
                    {
                        #region !shutdown (Admin only)
                        case "!shutdown":
                            if (!isBotAdmin(callBack.Sender))
                            {
                                steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Only admins can use the !shutdown command!");
                                break;
                            }
                            //Environment.Exit(0);
                            break;
                        #endregion

                        #region !resetadmins (Admin only)
                        case "!resetadmins":
                            if (!isBotAdmin(callBack.Sender))
                            {
                                steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Only admins can use the !resetadmins command!");
                                break;
                            }
                            try
                            {
                                steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Rechecking admin.txt...");

                                lines = File.ReadAllLines("admin.txt"); // Read all the SteamID64s in the file
                                admins.Clear(); // Clear the list to store the new admins
                                Console.WriteLine(lines);

                                for (int i = 0; i < lines.Length; i++) // Read each line in var
                                {
                                    try // Try to convert it
                                    {
                                        admins.Add(Convert.ToUInt64(lines[i])); // Add ID to list
                                    }
                                    catch // If invalid...
                                    {
                                        Console.WriteLine("WARNING: {0} is an invalid steamID16 in admin.txt! Skipping..."); // Tell the user
                                    }
                                }
                                steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Done!");
                            }catch (Exception e)
                            {
                                Console.WriteLine("Error while checking admin.txt: {0}", e.ToString());
                                steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, string.Format("Error while checking admin.txt: {0}", e.ToString()));
                            }
                            
                            break;
                        #endregion

                        #region !setup
                        case "!setup":
                            playerCount = players.Count;
                            for (var i = 0; i < playerCount; i++) {

                                if (players[i].id == callBack.Sender.AccountID)
                                {
                                    Console.WriteLine("!setup command recived. User: {0}", steamFriends.GetFriendPersonaName(callBack.Sender));
                                    steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Starting game...");
                                    players[i].setupGame();
                                    return;
                                }
                            }
                            steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Setting up user...");

                            players.Add(new Structures.PlayerStructure { id = callBack.Sender.AccountID });

                            for (var i = 0; i < players.Count; i++)
                            {
                                if (callBack.Sender.AccountID == players[i].id)
                                {
                                    players[i].setupGame();
                                }
                            }

                            break;

                        #endregion

                        #region !restart
                        case "!restart":
                            playerCount = players.Count;
                            for (var i = 0; i < playerCount; i++)
                            {

                                if (players[i].id == callBack.Sender.AccountID)
                                {
                                    Console.WriteLine("!restart command recived. User: {0}", steamFriends.GetFriendPersonaName(callBack.Sender));
                                    steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Restarting game...");
                                    players[i].setupGame();
                                    return;
                                }
                            }
                            steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "Setting up user...");

                            players.Add(new Structures.PlayerStructure { id = callBack.Sender.AccountID });

                            for (var i = 0; i < players.Count; i++)
                            {
                                if (callBack.Sender.AccountID == players[i].id)
                                {
                                    players[i].setupGame();
                                }
                            }
                            break;

                        #endregion

                        #region !attack
                        case "!attack":
                            foreach (Structures.PlayerStructure player in players) // Loop the list
                            {
                                if (player.id == callBack.Sender.AccountID)
                                {
                                    player.attack(callBack, steamFriends);
                                }
                            }
                            break;
                        #endregion

                        #region !shop
                        case "!shop":
                            foreach (Structures.PlayerStructure player in players) // Loop the list
                            {
                                if (player.id == callBack.Sender.AccountID)
                                {
                                    player.displayShop(callBack, steamFriends);
                                }
                            }
                            break;
                        #endregion

                        #region !status
                        case "!status":
                            foreach (Structures.PlayerStructure player in players) // Loop the list
                            {
                                if (player.id == callBack.Sender.AccountID)
                                {
                                    player.state(callBack, steamFriends);
                                }
                            }
                            break;
                        #endregion

                        #region !help
                        case "!help":
                            Console.WriteLine("!help command revied. User: {0}", steamFriends.GetFriendPersonaName(callBack.Sender));
                            steamFriends.SendChatMessage(callBack.Sender, EChatEntryType.ChatMsg, "\nThe current commands are:\n!help\n!attack\n!setup\n!status\n!shop\n!shutdown(admin only)\n!resetadmins(admin only)");
                            break;
                        #endregion
                    }
                }
                foreach (Structures.PlayerStructure player in players) // Loop the list
                {
                    if (player.id == callBack.Sender.AccountID && callBack.Message.Length == 1 && player.shopMode == true)
                    {
                        player.processShop(callBack.Message, callBack, steamFriends);
                    }
                }
            }
        }

        bool isBotAdmin(SteamID sid, bool refresh = false)
        {
            foreach (ulong id in admins) // Loop the list
            {
                if (sid.ConvertToUInt64() == id) // Convert the users steamID and see if it matches...
                {
                    return true; // Return true if id matched
                }
            }
            // If their ID was not found, tell the user
            steamFriends.SendChatMessage(sid, EChatEntryType.ChatMsg, "You are not a bot admin.");
            // And put the message in console
            Console.WriteLine(steamFriends.GetFriendPersonaName(sid) + " attempted to use an administrator command while not an administrator.");
            return false;
        }

        void OnFriendsAdded(SteamFriends.FriendsListCallback callBack)
        {
            Thread.Sleep(2500);

            foreach (var friend in callBack.FriendList)
            {
                if (friend.Relationship == EFriendRelationship.RequestRecipient)
                {
                    steamFriends.AddFriend(friend.SteamID);
                    Thread.Sleep(2000);
                    steamFriends.SendChatMessage(friend.SteamID, EChatEntryType.ChatMsg, "Hello, I am Battle Bot. Ask the owner to start a battle!");
                }
            }
        }

        string[] seperate(int number, char seperator, string thestring)
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

        public string inputPass()
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
    }
}
