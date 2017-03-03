using System;
using SteamKit2;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace SteamBattleBot.Structures
{
    class PlayerStructure
    {
        private Random _random = new Random();
        
        public EnemyStructure enemy = new EnemyStructure();

        public ulong id;

        [Newtonsoft.Json.JsonIgnore]
        public bool shopMode = false;

        private int hitChance,
                    damageDone,
                    damageTaken;

        public int hp,
                   coins,
                   skillPoints,
                   damageMultiplier,
                   maxHp,
                   level,
                   exp,
                   charge,
                   cooldown;

        public void SetupGame()
        {
            enemy.Reset();
            maxHp = 50;
            hp = 50;
            coins = 5;
            skillPoints = 0;
            damageMultiplier = 0;
            level = 1;
            exp = 0;
        }

        // Attack and Check which monster to attack
        public void Attack(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (shopMode == true)
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You must exit the shop! Type !shop to exit!");
            else
            {
                if (hp <= 0) //Stops the user from attacking if below 0 HP
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You cannot attack anymore! Type !restart to start a new game!");
                    return;
                }
                switch (enemy.type) {
                    case "Gaben Clone":
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 7);
                        if (hitChance == 4) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Gaben Clone!");
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10);
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You hit the Gaben Clone for {0} damage!", damageDone));
                        }
                        hitChance = _random.Next(1, 4);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Gaben Clone missed you!");
                        }
                        else
                        {
                            damageTaken = _random.Next(1, 15); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone hit you with the Ban Hammer for {0} damage!", damageTaken));
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        Stats(callback, steamFriends, true);
                        break;

                    case "Steam Bot":
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 7);
                        if (hitChance == 4) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Steam Bot!");
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10);
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You hit the Steam Bot for {0} damage!", damageDone));
                        }
                        hitChance = _random.Next(1, 4);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Bot missed you!");
                        }
                        else
                        {
                            damageTaken = _random.Next(1, 5); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot spammed you with phishing links for {0} damage!", damageTaken));
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        Stats(callback, steamFriends, true);
                        break;

                    case "Steam Mod":
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 7);
                        if (hitChance == 4) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Steam Mod!");
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10);
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You hit the Steam Mod for {0} damage!", damageDone));
                        }
                        hitChance = _random.Next(1, 4);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Mod missed you!");
                        }
                        else
                        {
                            damageTaken = _random.Next(1, 10); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod blocks you from posting for {0} damage!", damageTaken));
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        Stats(callback, steamFriends, true);
                        break;
                    case "Steam Admin":
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 7);
                        if (hitChance == 4) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Steam Admin!");
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10);
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You hit the Steam Admin for {0} damage!", damageDone));
                        }
                        hitChance = _random.Next(1, 4);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Admin missed you!");
                        }
                        else
                        {
                            damageTaken = _random.Next(5, 10); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin bans you for trading for {0} damage!", damageTaken));
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        Stats(callback, steamFriends, true);
                        break;
                }
            }
        }

        // Block the attacks
        public void Block(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (shopMode == true)
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You must exit the shop! Type !shop to exit!");
            else
            {
                if (hp <= 0) //Stops the user from attacking if below 0 HP
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You cannot block anymore! Type !restart to start a new game!");
                    return;
                }

                #region Checks if Enemy is charging attack
                if (enemy.charge == 1)
                {
                    switch (enemy.type) {
                        case "Gaben Clone":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 5);
                            if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Gaben Clone's special attack!");
                                damageTaken = _random.Next(1, 15) * 2; // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone deletes {0} of your skins!", damageTaken));
                                enemy.charge = 0;
                                cooldown -= 1;
                            }
                            else
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Gaben Clone's special attack!"));
                                enemy.charge = 0;
                                cooldown -= 1;

                            }
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;

                        case "Steam Bot":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 5);
                            if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Steam Bot's special attack!");
                                damageTaken = _random.Next(1, 5) * 2; // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot hijacked your inventory! It takes {0} days to recover your items!", damageTaken));
                                enemy.charge = 0;
                                cooldown -= 1;
                            }
                            else
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Steam Bot's special attack!"));
                                enemy.charge = 0;
                                cooldown -= 1;
                            }
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;

                        case "Steam Mod":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 5);
                            if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Steam Mod's special attack!");
                                damageTaken = _random.Next(1, 10) * 2; // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod ignores all your support tickets. It takes {0} months before getting a respond!", damageTaken));
                                enemy.charge = 0;
                                cooldown -= 1;
                            }
                            else
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Steam Mod's special attack!"));
                                enemy.charge = 0;
                                cooldown -= 1;
                            }
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;
                        case "Steam Admin":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 5);
                            if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Steam Admin's special attack!");
                                damageTaken = _random.Next(5, 10) * 2; // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin bans you from Steam Community for {0} days!", damageTaken));
                                enemy.charge = 0;
                                cooldown -= 1;
                            }
                            else
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Steam Admin's special attack!"));
                                enemy.charge = 0;
                                cooldown -= 1;

                            }
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;
                    }
                }
                #endregion
                #region Block regular attacks
                else
                {
                    switch (enemy.type) {
                        case "Gaben Clone":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 5);
                            if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Gaben Clone's attack!");
                                damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone hit you with the Ban Hammer for {0} damage!", damageTaken));
                            }
                            else
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Gaben Clone's attack!"));
                            }
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;

                        case "Steam Bot":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 5);
                            if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Steam Bot's attack!");
                                damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot spammed you with phishing links for {0} damage!", damageTaken));
                            }
                            else
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Steam Bot's attack!"));
                            }
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;

                        case "Steam Mod":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 5);
                            if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Steam Mod's attack!");
                                damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod blocks you from posting for for {0} damage!", damageTaken));
                            }
                            else
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Steam Mod's attack!"));
                            }
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;

                        case "Steam Admin":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 5);
                            if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Steam Admin's attack!");
                                damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin bans you from trading for {0} damage!", damageTaken));
                            }
                            else
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Steam Admin's attack!"));
                            }
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;
                    }
                }
                #endregion
            }
        }

        // Special attack handling
        public void Special(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (shopMode == true)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You must exit the shop! Type !shop to exit!");
                return;
            }
            else
            {
                if (hp <= 0) //Stops the user from attacking if below 0 HP
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You cannot use special attack anymore! Type !restart to start a new game!");
                    return;
                }
                #region If cooldown isn't active
                if (charge == 0)
                {
                    switch (enemy.type)
                    {
                        case "Gaben Clone":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 4);
                            charge += 1;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                            if (hitChance == 3) // Missed
                            {
                                damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone hit you with the Ban Hammer for {0} damage!", damageTaken));
                            }
                            else
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone missed you!"));
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;

                        case "Steam Bot":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 4);
                            charge += 1;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                            if (hitChance == 3) // Missed
                            {
                                damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot spammed you with phishing links for {0} damage!", damageTaken));
                            }
                            else
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot missed you!"));
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;

                        case "Steam Mod":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 4);
                            charge += 1;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                            if (hitChance == 3) // Missed
                            {
                                damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod blocks you from posting for for {0} damage!", damageTaken));
                            }
                            else
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod missed you!"));
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;

                        case "Steam Admin":
                            #region Miss/Hit checker
                            hitChance = _random.Next(1, 4);
                            charge += 1;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                            if (hitChance == 3) // Missed
                            {
                                damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin bans you from trading for {0} damage!", damageTaken));
                            }
                            else
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin missed you!"));
                            #endregion

                            HpCheck(callback, steamFriends);
                            Stats(callback, steamFriends, true);
                            break;
                    }
                    return;
                }
                #endregion

                // Starting to charge
                if (cooldown >= 1)
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have to wait before using a special attack again!");
                    cooldown -= 1;
                    return;
                }
                
            }

            #region Charge #1 Attack
            if (charge == 1)
            {
                switch (enemy.type) {
                    case "Gaben Clone":
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 5);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed your special attack!");
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have to wait before using a special attack again!");
                            charge = 0;
                            cooldown = 2;
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10) * 2;
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You released HL3 and hurt Gaben Clone's feelings for {0} damage!", damageDone));
                            charge = 0;
                            cooldown = 2;
                        }
                        #endregion

                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 4);
                        if (enemy.charge == 1)
                        {
                            damageTaken = _random.Next(1, 15) * 2; // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone deletes {0} of your skins!", damageTaken));
                            enemy.charge = 0;
                        }
                        else
                        {
                            if (hitChance == 2) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Gaben Clone charging his attack!");
                                enemy.charge += 1;
                            }
                            else if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Gaben Clone missed you!");
                            }
                            else
                            {
                                damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone hit you with the Ban Hammer for {0} damage!", damageTaken));
                            }
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        Stats(callback, steamFriends, true);
                        break;

                    case "Steam Bot":
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 5);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed your special attack!");
                            charge = 0;
                            cooldown = 2;
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10) * 2;
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You reported the Steam Bot. The Steam Bot been banned for {0} days!", damageDone));
                            charge = 0;
                            cooldown = 2;
                        }
                        #endregion

                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 4);
                        if (enemy.charge == 1)
                        {
                            damageTaken = _random.Next(1, 5) * 2; // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot hijacked your inventory! It takes {0} days to recover your items!", damageTaken));
                            enemy.charge = 0;
                        }
                        else
                        {
                            if (hitChance == 2) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Bot keylogging your PC!");
                                enemy.charge += 1;
                            }
                            else if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Bot missed you!");
                            }
                            else
                            {
                                damageTaken = _random.Next(1, 5); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot spams you with phishing links for {0} damage!", damageTaken));
                            }
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        Stats(callback, steamFriends, true);
                        break;

                    case "Steam Mod":
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 5);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed your special attack!");
                            charge = 0;
                            cooldown = 2;
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10) * 2;
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You exposed the Steam Mod for corruptions. The Steam Mod recieves hate for {0} days!", damageDone));
                            charge = 0;
                            cooldown = 2;
                        }
                        #endregion

                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 4);
                        if (enemy.charge == 1)
                        {
                            damageTaken = _random.Next(1, 10) * 2; // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod ignores all your support tickets. It takes {0} months before getting a respond!", damageTaken));
                            enemy.charge = 0;
                        }
                        else
                        {
                            if (hitChance == 2) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Mod logging into Steam Support!");
                                enemy.charge += 1;
                            }
                            else if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Mod missed you!");
                            }
                            else
                            {
                                damageTaken = _random.Next(1, 10); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod blocks you from posting for {0} damage!", damageTaken));
                            }
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        Stats(callback, steamFriends, true);
                        break;

                    case "Steam Admin":
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 5);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed your special attack!");
                            charge = 0;
                            cooldown = 2;
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10) * 2;
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You exposed the Steam Admin for corruptions. The Steam Admin recieves hate for {0} days!", damageDone));
                            charge = 0;
                            cooldown = 2;
                        }
                        #endregion

                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 4);
                        if (enemy.charge == 1)
                        {
                            damageTaken = _random.Next(5, 10) * 2; // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin bans you from Steam Community for {0} days!", damageTaken));
                            enemy.charge = 0;
                        }
                        else
                        {
                            if (hitChance == 2) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Mod browsing your profile!");
                                enemy.charge += 1;
                            }
                            else if (hitChance == 3) // Missed
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Admin missed you!");
                            }
                            else
                            {
                                damageTaken = _random.Next(5, 10); // How much damage did the monster do
                                hp -= damageTaken;
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin bans you for trading for {0} damage!", damageTaken));
                            }
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        Stats(callback, steamFriends, true);
                        break;

                }
                
            }
            #endregion
        }

        // Check to see if the player can level up
        private void LevelUp(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (exp == 10)
            {
                level += 1;
                coins += 15;
                skillPoints += 3;
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Congrats, you are level {0} and earned 15 coins and 3 points!", level));
                exp = 0;
            }
        }

        public void Stats(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends, bool miniPrint = false)
        {
            if (miniPrint && !shopMode)
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, string.Format("\nYou have {0} HP!\nThe {1} has {2} HP!", hp, enemy.type, enemy.hp));
            else
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, 
                "\nCurrent Stats\n"+
                  "Max HP: " +maxHp+"\n"+
                  "Current HP: " +hp+ "\n" +
                  "Level: " +level+ "\n" +
                  "XP: " +exp+ "\n" +
                  "Coins: " +coins+ "\n" +
                  "Points: " +skillPoints+ "\n" +
                  "Damage Increased: +" +damageMultiplier+ "\n" +
                "\n"+enemy.type+" Stats\n"+
                  "HP: " +enemy.hp);
        }

        // Check if battle is won/game over.
        private bool HpCheck(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            switch (enemy.type) {
                case "Steam Bot":
                    if (hp <= 0)
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Bot stolen your account! Type !restart to start a new game.");
                        return true;
                    }
                    else if (enemy.hp <= 0)
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You killed the Steam Bot! The Steam Bot dropped {0} coins and you earned {1} XP. Making new enemy...", enemy.coins, enemy.exp));
                        coins += enemy.coins;
                        exp += enemy.exp;
                        hp += _random.Next(1, 6);
                        enemy.Reset();
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You are facing a {0}!", enemy.type));
                        return true;
                    }
                    break;
                case "Gaben Clone":
                    if (hp <= 0)
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Gaben Clone banned you from Steam! Type !restart to start a new game.");
                        return true;
                    }
                    else if (enemy.hp <= 0)
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You killed the Gaben Clone! He dropped {0} skill points and you earned {1} XP. Making new enemy...", enemy.points, enemy.exp));
                        skillPoints += enemy.points;
                        exp += enemy.exp;
                        hp += _random.Next(1, 6);
                        enemy.Reset();
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You are facing a {0}!", enemy.type));
                        return true;
                    }
                    break;
                case "Steam Mod":
                    if (hp <= 0)
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Mod banned you from Steam Forums! Type !restart to start a new game.");
                        return true;
                    }
                    else if (enemy.hp <= 0)
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You killed the Steam Mod! The Steam Mod dropped {0} coins and you earned {1} XP. Making new enemy...", enemy.coins, enemy.exp));
                        coins += enemy.coins;
                        exp += enemy.exp;
                        hp += _random.Next(1, 6);
                        enemy.Reset();
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You are facing a {0}!", enemy.type));
                        return true;
                    }
                    break;
                case "Steam Admin":
                    if (hp <= 0)
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Steam Admin banned you from trading! RIP Skins! Type !restart to start a new game.");
                        return true;
                    }
                    else if (enemy.hp <= 0)
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You killed the Steam Admin! The Steam Admin dropped {0} coins and you earned {1} XP. Making new enemy...", enemy.coins, enemy.exp));
                        coins += enemy.coins;
                        exp += enemy.exp;
                        hp += _random.Next(1, 6);
                        enemy.Reset();
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You are facing a {0}!", enemy.type));
                        return true;
                    }
                    break;
            }
            return false;
        }

        // Show the shop menu
        public void DisplayShop(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (!shopMode)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg,
                    "\nWelcome to the shop! You got "+coins+" coin"+ (coins == 1 ? "" : "s") +" and " +skillPoints+ " skill point"+ (skillPoints == 1 ? "" : "s") + "\n" + 
                    "1. Heal 5 hp: 2 coins\n" + // ^ This will need to fixed once I get the string builder method working ^
                    "2. Heal 10 hp: 5 coins\n" +
                    "3. Insta-kill: 10 coins\n" +
                    "4. Increase Damage:  1 skill point\n" +
                    "5. Increase HP by 10: 1 skill point\n" +
                    "Simple type in the number of the item you want.\n" +
                    "To exit, type !shop again.");
                shopMode = true;
            }
            else {
                shopMode = false;
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have exited the shop...");
            }
        }
        
        // Process item requests
        public void ProcessShop(string item, SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            switch (item)
            {
                #region Heal 5
                case "1":
                    if (hp < maxHp)
                    {
                        if (coins >= 2)
                        {
                            coins -= 2;
                            hp += 5;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You healed yourself for 5 HP, spending 2 coins!");
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + coins + " coin" + (coins == 1 ? "" : "s") + " left.");
                        }
                        else
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough coins. You only have " + coins + " coin" + (coins == 1 ? "" : "s") + " in your wallet.");
                        }
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You already at max HP!");
                    }
                    break;
                #endregion

                #region Heal 10
                case "2":
                    if (hp < maxHp)
                    {
                        if (coins >= 5)
                        {
                            coins -= 5;
                            hp += 10;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You healed yourself for 10 HP, spending 5 coins!");
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + coins + " coin" + (coins == 1 ? "" : "s") + " left.");
                        }
                        else
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough coins. You only have " + coins + " coin" + (coins == 1 ? "" : "s") + " in your wallet.");
                        }
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You already at max HP!");
                    }
                    break;
                #endregion

                #region Insta-kill
                case "3":
                    if (coins >= 10)
                    {
                        coins -= 10;
                        enemy.hp = 0;
                        shopMode = false;
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "Insta-killing enemy...");
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + coins + " coin" + (coins == 1 ? "" : "s") + " left.");
                        HpCheck(callback, steamFriends);
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough coins. You only have " + coins + " coin" + (coins == 1 ? "" : "s") + " in your wallet.");
                    }
                    break;
                #endregion

                #region Damage Increase
                case "4":
                    if (skillPoints >= 1)
                    {
                        skillPoints -= 1;
                        damageMultiplier += 3;
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You increased your damage.");
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + skillPoints + " point" + (skillPoints == 1 ? "" : "s") + " left.");
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough points. You only have " + skillPoints + " point" + (skillPoints == 1 ? "" : "s") + " in your wallet.");
                    }
                    break;
                #endregion

                #region HP Increase
                case "5":
                    if (skillPoints >= 1)
                    {
                        skillPoints -= 1;
                        maxHp += 10;
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You increased your HP by 10.");
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + skillPoints + " point" + (skillPoints == 1 ? "" : "s") + " left.");
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough points. You only have " + skillPoints + " point" + (skillPoints == 1 ? "" : "s") + " in your wallet.");
                    }
                    break;
                    #endregion
            }
        }
    }
}
