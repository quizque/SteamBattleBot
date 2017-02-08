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

        private int hp, coins, hitChance, damageDone, points, damageTaken, damageMultiplier, maxhp, level, exp;


        public void setupGame()
        {
            enemy.Reset();
            maxhp = 50;
            hp = 50;
            coins = 5;
            points = 0;
            damageMultiplier = 1;
            damageDone = _random.Next(1, 10);
            level = 1;
            exp = 0;
        }

        #region Attack and Check which monster to attack
        public void attack(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (!shopMode)
            {
                if (hp <= 0) //Stops the user from attacking if below 0 HP
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You cannot attack anymore! Type !restart to start a new game!");
                }
                else
                {
                    if (enemy.classRandom == 0)
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You are facing a Gaben Clone.");
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 4);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Gaben Clone!");
                        }
                        else
                        {
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
                            damageTaken = _random.Next(1, 20); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Gaben Clone hitted you with the Ban Hammer for {0} damage!", damageTaken));
                        }
                        #endregion

                        hpCheck(callback, steamFriends);
                        state(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 1)
                    {
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 4);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Steam Bot!");
                        }
                        else
                        {
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
                            damageTaken = _random.Next(1, 14); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Bot spammed you with phishing links for {0} damage!", damageTaken));
                        }
                        #endregion

                        hpCheck(callback, steamFriends);
                        state(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 2)
                    {
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 4);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Steam Mod!");
                        }
                        else
                        {
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
                            damageTaken = _random.Next(1, 16); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Mod blocks you from posting for {0} damage!", damageTaken));
                        }
                        #endregion

                        hpCheck(callback, steamFriends);
                        state(callback, steamFriends);
                    }
                    else if (enemy.classRandom == 3)
                    {
                        #region Miss/Hit checker
                        hitChance = _random.Next(1, 4);
                        if (hitChance == 3) // Missed
                        {
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Steam Admin!");
                        }
                        else
                        {
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
                            damageTaken = _random.Next(1, 18); // How much damage did the monster do
                            hp -= damageTaken;
                            steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Steam Admin bans you for trading for {0} damage!", damageTaken));
                        }
                        #endregion

                        hpCheck(callback, steamFriends);
                        state(callback, steamFriends);
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Something is wrong with the bot. Please contact the owner of the bot."));
                    }
                }
            }
        }
        #endregion

        #region Check the status of the enemy
        public void state(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (enemy.classRandom == 1)
            {
                if (!shopMode)
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("\nCurrent Status\nYou:\nHP: {0}\nLevel: {1}\nXP: {2}\nCoins: {3}\nPoints: {4}\nSteam Bot:\nHP: {5}", hp, level, exp, coins, points, enemy.hp));
            }
            else if (enemy.classRandom == 0)
            {
                if (!shopMode)
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("\nCurrent Status\nYou:\nHP: {0}\nLevel: {1}\nXP: {2}\nCoins: {3}\nPoints: {4}\nGaben Clone:\nHP: {5}", hp, level, exp, coins, points, enemy.hp));
            }
            else if (enemy.classRandom == 2)
            {
                if (!shopMode)
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("\nCurrent Status\nYou:\nHP: {0}\nLevel: {1}\nXP: {2}\nCoins: {3}\nPoints: {4}\nSteam Mod:\nHP: {5}", hp, level, exp, coins, points, enemy.hp));
            }
            else if (enemy.classRandom == 3)
            {
                if (!shopMode)
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("\nCurrent Status\nYou:\nHP: {0}\nLevel: {1}\nXP: {2}\nCoins: {3}\nPoints: {4}\nSteam Admin:\nHP: {5}", hp, level, exp, coins, points, enemy.hp));
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
                points += 5;
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("Congrats, you are level {0} and earned 15 coins and 5 points!", level));
                exp = 0;
            }
        }

        #region Check if battle is won/game over.
        private bool hpCheck(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
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
                    points += enemy.points;
                    exp += enemy.exp;
                    hp += _random.Next(1, 6);
                    enemy.Reset();
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

        public void displayShop(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (!shopMode)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg,
                    string.Format(
                    "\nWelcome to the shop! You got {0} coins and " +points+ " points\n", coins) +
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

        public void processShop(string item, SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            switch (item)
            {
                #region Heal 5
                case "1":
                    if (hp < maxhp)
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
                    if (hp < maxhp)
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
                        hpCheck(callback, steamFriends);
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough coins. You only have " + coins + " coins in your wallet.");
                    }
                    break;
                #endregion

                #region Damage Increase
                case "4":
                    if (points >= 1)
                    {
                        points -= 1;
                        damageMultiplier *= 2;
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You increased your damage.");
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + points + " points left.");
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough points. You only have " + points + " points in your wallet.");
                    }
                    break;
                #endregion

                #region HP Increase
                case "5":
                    if (points >= 1)
                    {
                        points -= 1;
                        maxhp += 10;
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You increased your HP by 10.");
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + points + " points left.");
                    }
                    else
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You don't have enough points. You only have " + points + " points in your wallet.");
                    }
                    break;
                    #endregion
            }
        }
    }
}
