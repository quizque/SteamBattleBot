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

        private EnemyStructure enemy = new EnemyStructure();

        public ulong id;

        public bool shopMode = false;

        private int hp,
                    coins,
                    hitChance,
                    damageDone,
                    skillPoints,
                    damageTaken,
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
            charge = 0;
            cooldown = 0;
        }

        #region Attack and Check which monster to attack
        public void Attack(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (shopMode == true)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You must exit the shop! Type !shop to exit!");
            }
            #region Checks if enemy is charged
            else
            {
                if (hp <= 0) //Stops the user from attacking if below 0 HP
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You cannot attack anymore! Type !restart to start a new game!");
                }
                else
                {
                    if (enemy.classRandom == 0)
                    {
                        hitChance = _random.Next(1, 7);
                        if (hitChance == 4) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Gaben Clone!");
                            cooldown -= 1;
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10);
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You hit the Gaben Clone for {0} damage!", damageDone));
                            cooldown -= 1;
                        }

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

                        HpCheck(callback, steamFriends);
                        State(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 1)
                    {

                        hitChance = _random.Next(1, 7);
                        if (hitChance == 4) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Steam Bot!");
                            cooldown -= 1;
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10);
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You hit the Steam Bot for {0} damage!", damageDone));
                            cooldown -= 1;
                        }

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


                        HpCheck(callback, steamFriends);
                        State(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 2)
                    {
                       
                        hitChance = _random.Next(1, 7);
                        if (hitChance == 4) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Steam Mod!");
                            cooldown -= 1;
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10);
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You hit the Steam Mod for {0} damage!", damageDone));
                            cooldown -= 1;
                        }
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

                        HpCheck(callback, steamFriends);
                        State(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 3)
                    {
                        hitChance = _random.Next(1, 7);
                        if (hitChance == 4) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Steam Admin!");
                            cooldown -= 1;
                        }
                        else
                        {
                            damageDone = _random.Next(1, 10);
                            damageDone += damageMultiplier;
                            enemy.hp -= damageDone;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You hit the Steam Admin for {0} damage!", damageDone));
                            cooldown -= 1;
                        }
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
                     
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Something is wrong with the bot. Please contact the owner of the bot."));
                    }
                    #endregion
                }
            }
        }
        #endregion

        #region Block the attacks
        public void Block(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (shopMode == true)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You must exit the shop! Type !shop to exit!");
            }
            else
            {
                if (hp <= 0) //Stops the user from attacking if below 0 HP
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You cannot block anymore! Type !restart to start a new game!");
                }
                #region Checks if Enemy is charging attack
                else if (enemy.charge == 1)
                {
                    if (enemy.classRandom == 0)
                    {
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
                        State(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 1)
                    {
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
                        State(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 2)
                    {
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
                        State(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 3)
                    {
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
                        State(callback, steamFriends);
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Something is wrong with the bot. Please contact the owner of the bot."));
                    }
                }
                #endregion
                #region Block regular attacks
                else
                {
                    if (enemy.classRandom == 0)
                    {
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 5);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Gaben Clone's attack!");
                            damageTaken = _random.Next(1, 15); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone hit you with the Ban Hammer for {0} damage!", damageTaken));
                            cooldown -= 1;
                        }
                        else
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Gaben Clone's attack!"));
                            cooldown -= 1;

                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        State(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 1)
                    {
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 5);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Steam Bot's attack!");
                            damageTaken = _random.Next(1, 5); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot spammed you with phishing links for {0} damage!", damageTaken));
                            cooldown -= 1;
                        }
                        else
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Steam Bot's attack!"));
                            cooldown -= 1;
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        State(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 2)
                    {
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 5);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Steam Mod's attack!");
                            damageTaken = _random.Next(1, 10); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod blocks you from posting for for {0} damage!", damageTaken));
                            cooldown -= 1;
                        }
                        else
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Steam Mod's attack!"));
                            cooldown -= 1;
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        State(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 3)
                    {
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 5);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You failed to block Steam Admin's attack!");
                            damageTaken = _random.Next(5, 10); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin bans you from trading for {0} damage!", damageTaken));
                            cooldown -= 1;
                        }
                        else
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You blocked Steam Admin's attack!"));
                            cooldown -= 1;
                        }
                        #endregion

                        HpCheck(callback, steamFriends);
                        State(callback, steamFriends);
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Something is wrong with the bot. Please contact the owner of the bot."));
                    }
                    #endregion
                }
            }
        }
        #endregion

#region Special attacks
        public void Special(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (shopMode == true)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You must exit the shop! Type !shop to exit!");
            }
            else
            {
                if (hp <= 0) //Stops the user from attacking if below 0 HP
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You cannot use special attack anymore! Type !restart to start a new game!");
                }
                else
                {
                    #region Starting to charge
                    if (cooldown >= 1)
                    {
                        if (enemy.classRandom == 0)
                        {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have to wait before using a special attack again!");
                                cooldown -= 1;

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
                            State(callback, steamFriends);
                        }
                        else if (enemy.classRandom == 1)
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have to wait before using a special attack again!");
                            cooldown -= 1;

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
                            State(callback, steamFriends);
                        }
                        else if (enemy.classRandom == 2)
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have to wait before using a special attack again!");
                            cooldown -= 1;

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
                            State(callback, steamFriends);
                        }
                        else if (enemy.classRandom == 3)
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have to wait before using a special attack again!");
                            cooldown -= 1;

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
                        }
                        else
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Something is wrong with the bot. Please contact the owner of the bot."));
                        }
                        #endregion
                    }
                    #region If cooldown isn't active
                    else
                    {
                        if (charge == 0)
                        {
                            if (enemy.classRandom == 0)
                            {
                                #region Miss/Hit checker
                                hitChance = _random.Next(1, 4);
                                if (hitChance == 3) // Missed
                                {
                                    charge += 1;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                                    damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                    hp -= damageTaken;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone hit you with the Ban Hammer for {0} damage!", damageTaken));
                                }
                                else
                                {
                                    charge += 1;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone missed you!"));
                                }
                                #endregion

                                HpCheck(callback, steamFriends);
                                State(callback, steamFriends);
                            }
                            else if (enemy.classRandom == 1)
                            {
                                #region Miss/Hit checker
                                hitChance = _random.Next(1, 4);
                                if (hitChance == 3) // Missed
                                {
                                    charge += 1;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                                    damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                    hp -= damageTaken;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot spammed you with phishing links for {0} damage!", damageTaken));
                                }
                                else
                                {
                                    charge += 1;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot missed you!"));
                                }
                                #endregion

                                HpCheck(callback, steamFriends);
                                State(callback, steamFriends);
                            }
                            else if (enemy.classRandom == 2)
                            {
                                #region Miss/Hit checker
                                hitChance = _random.Next(1, 4);
                                if (hitChance == 3) // Missed
                                {
                                    charge += 1;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                                    damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                    hp -= damageTaken;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod blocks you from posting for for {0} damage!", damageTaken));
                                }
                                else
                                {
                                    charge += 1;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod missed you!"));
                                }
                                #endregion

                                HpCheck(callback, steamFriends);
                                State(callback, steamFriends);
                            }
                            else if (enemy.classRandom == 3)
                            {
                                #region Miss/Hit checker
                                hitChance = _random.Next(1, 4);
                                if (hitChance == 3) // Missed
                                {
                                    charge += 1;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                                    damageTaken = _random.Next(1, 15); // How much damage did the monster do
                                    hp -= damageTaken;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin bans you from trading for {0} damage!", damageTaken));
                                }
                                else
                                {
                                    charge += 1;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You charging your special attack! Charges: {0}", charge));
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin missed you!"));
                                }
                                #endregion

                                HpCheck(callback, steamFriends);
                                State(callback, steamFriends);
                            }
                            else
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Something is wrong with the bot. Please contact the owner of the bot."));
                            }
                        }
                        #endregion
                        #region Charge #1 Attack
                        else if (charge == 1)
                        {
                            if (enemy.classRandom == 0)
                            {
                                #region Miss/Hit checker
                                hitChance = _random.Next(1, 5);
                                if (hitChance == 3) // Missed
                                {
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed your special attack!");
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have to wait before using a special attack again!");
                                    charge = 0;
                                    cooldown = 3;
                                }
                                else
                                {
                                    damageDone = _random.Next(1, 10) * 2;
                                    damageDone += damageMultiplier;
                                    enemy.hp -= damageDone;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You released HL3. You hurt Gaben Clone's feelings for {0} damage!", damageDone));
                                    charge = 0;
                                    cooldown = 3;
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
                                State(callback, steamFriends);
                            }
                            else if (enemy.classRandom == 1)
                            {
                                #region Miss/Hit checker
                                hitChance = _random.Next(1, 5);
                                if (hitChance == 3) // Missed
                                {
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed your special attack!");
                                    charge = 0;
                                    cooldown = 3;
                                }
                                else
                                {
                                    damageDone = _random.Next(1, 10) * 2;
                                    damageDone += damageMultiplier;
                                    enemy.hp -= damageDone;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You reported the Steam Bot. The Steam Bot been banned for {0} days!", damageDone));
                                    charge = 0;
                                    cooldown = 3;
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
                                State(callback, steamFriends);
                            }
                            else if (enemy.classRandom == 2)
                            {
                                #region Miss/Hit checker
                                hitChance = _random.Next(1, 5);
                                if (hitChance == 3) // Missed
                                {
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed your special attack!");
                                    charge = 0;
                                    cooldown = 3;
                                }
                                else
                                {
                                    damageDone = _random.Next(1, 10) * 2;
                                    damageDone += damageMultiplier;
                                    enemy.hp -= damageDone;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You exposed the Steam Mod for corruptions. The Steam Mod recieves hate for {0} days!", damageDone));
                                    charge = 0;
                                    cooldown = 3;
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
                                State(callback, steamFriends);
                            }
                            else if (enemy.classRandom == 3)
                            {
                                #region Miss/Hit checker
                                hitChance = _random.Next(1, 5);
                                if (hitChance == 3) // Missed
                                {
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed your special attack!");
                                    charge = 0;
                                    cooldown = 3;
                                }
                                else
                                {
                                    damageDone = _random.Next(1, 10) * 2;
                                    damageDone += damageMultiplier;
                                    enemy.hp -= damageDone;
                                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You exposed the Steam Admin for corruptions. The Steam Admin recieves hate for {0} days!", damageDone));
                                    charge = 0;
                                    cooldown = 3;
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
                            }
                            else
                            {
                                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Something is wrong with the bot. Please contact the owner of the bot."));
                            }
                            #endregion
                        }
                    }
                  }
                }
            }
            #endregion


        #region Check the status of the enemy
        public void State(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (enemy.classRandom == 1)
            {
                if (!shopMode)
                { 
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You have {0} HP!", hp));
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Steam Bot have {0} HP!", enemy.hp));
                }
            }
            else if (enemy.classRandom == 0)
            {
                if (!shopMode)
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You have {0} HP!", hp));
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Gaben Clone have {0} HP!", enemy.hp));
                }
            }
            else if (enemy.classRandom == 2)
            {
                if (!shopMode)
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You have {0} HP!", hp));
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Steam Mod have {0} HP!", enemy.hp));
                }
            }
            else if (enemy.classRandom == 3)
            {
                if (!shopMode)
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You have {0} HP!", hp));
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Steam Admin have {0} HP!", enemy.hp));
                }
            }
            else
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Something is wrong with the bot. Please contact the owner of the bot."));
            }
        }
        #endregion

        private void levelup(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (exp == 50)
            {
                level += 1;
                coins += 15;
                skillPoints += 5;
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Congrats, you are level {0} and earned 15 coins and 5 points!", level));
                exp = 0;
            }
        }

        public void Stats(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("\nCurrent Stats\nMax HP: {0}\nLevel: {1}\nXP: {2}\nCoins: {3}\nPoints: {4}\nDamage Increased: +{5}", maxHp, level, exp, coins, skillPoints, damageMultiplier));
        }

        #region Check if battle is won/game over.
        private bool HpCheck(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (enemy.classRandom == 1)
            {
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
            }
            else if (enemy.classRandom == 0)
            {
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
            }
            else if (enemy.classRandom == 2)
            {
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
            }
            else if (enemy.classRandom == 3)
            {
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
            }
            else
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Something is wrong with the bot. Please contact the owner of the bot."));
            }
            return false;
        }
        #endregion

        public void DisplayShop(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (!shopMode)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg,
                    string.Format(
                    "\nWelcome to the shop! You got {0} coins and " +skillPoints+ " points\n", coins) +
                    "1. Heal 5 hp: 3 coins\n" +
                    "2. Heal 10 hp: 5 coins\n" +
                    "3. Insta-kill: 10 coins\n" +
                    "4. Increase Damage:  1 point\n" +
                    "5. Increase HP by 10: 1 point\n" +
                    "Simple type in the number of the item you want.\n" +
                    "To exit, type !shop again.");
                shopMode = true;
            }
            else {
                shopMode = false;
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have exited the shop...");
            }
        }

        public void ProcessShop(string item, SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            switch (item)
            {
                #region Heal 5
                case "1":
                    if (hp < maxHp)
                    {
                        if (coins >= 3)
                        {
                            coins -= 3;
                            hp += 5;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You healed yourself for 5 HP, spending 3 coins!");
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + coins + " coins left.");
                        }
                        else
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough coins. You only have " + coins + " coins in your wallet.");
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
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + coins + " coins left.");
                        }
                        else
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough coins. You only have " + coins + " coins in your wallet.");
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
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + coins + " coins left.");
                        HpCheck(callback, steamFriends);
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough coins. You only have " + coins + " coins in your wallet.");
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
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + skillPoints + " points left.");
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough points. You only have " + skillPoints + " points in your wallet.");
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
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + skillPoints + " points left.");
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough points. You only have " + skillPoints + " points in your wallet.");
                    }
                    break;
                    #endregion
            }
        }
    }
}
