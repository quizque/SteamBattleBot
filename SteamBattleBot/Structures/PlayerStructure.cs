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

        private int hp, coins, hitChance, damageDone, points, damageTaken, damageMultiplier, maxhp;


        public void setupGame()
        {
            enemy.Reset();
            maxhp = 50;
            hp = 50;
            coins = 5;
            points = 0;
            damageMultiplier = 1;
            damageDone = _random.Next(1, 10);
        }

        public void attack(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (!shopMode)
            {
                if ((enemy.hp >=40) && (enemy.hp <= 50))
                {
                    enemy.hp = enemy.hpboss;
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You are facing a Gaben Clone.");
                    #region Miss/Hit checker
                    hitChance = _random.Next(1, 4);
                    if (hitChance == 3) // Missed
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Gaben Clone!");
                    }
                    else
                    {
                        enemy.hpboss -= damageDone;
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

                    hpCheckGaben(callback, steamFriends);

                    stateGaben(callback, steamFriends);
                }
                else if (enemy.hp < 40)
                {
                    #region Miss/Hit checker
                    hitChance = _random.Next(1, 4);
                    if (hitChance == 3) // Missed
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the Robot!");
                    }
                    else
                    {
                        enemy.hp -= damageDone;
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You hit the Robot for {0} damage!", damageDone));
                    }
                    hitChance = _random.Next(1, 4);
                    if (hitChance == 3) // Missed
                    {
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Robot missed you!");
                    }
                    else
                    {
                        damageTaken = _random.Next(1, 12); // How much damage did the monster do
                        hp -= damageTaken;
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The Robot hit you for {0} damage!", damageTaken));
                    }
                    #endregion

                    hpCheck(callback, steamFriends);

                    state(callback, steamFriends);

                }
            }
        }

        public void state(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (!shopMode)
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("\nCurrent Status\nYou:\nHP: {0}\nCoins: {1}\nPoints: {3}\nRobot:\nHP: {2}", hp, coins, enemy.hp, points));
        }
        public void stateGaben(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (!shopMode)
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("\nCurrent Status\nYou:\nHP: {0}\nCoins: {1}\nGaben Clone:\nHP: {2}", hp, coins, enemy.hpboss));
        }

        private bool hpCheck(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            // Return true if game over
            // Return false if not
            if (hp <= 0)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Robot crushed your bones! Restarting game...");
                setupGame();
                return true;
            }
            else if (enemy.hp <= 0)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You killed the Robot! He dropped {0} coins. Making new Robot...", enemy.coins));
                coins += enemy.coins;
                hp += _random.Next(1, 6);
                enemy.Reset();
                return true;
            }
            return false;
        }

        private bool hpCheckGaben(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            // Return true if game over
            // Return false if not
            if (hp <= 0)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The Gaben Clone banned you from Steam! Restarting game...");
                setupGame();
                return true;
            }else if (enemy.hpboss <= 0)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You killed the Gaben Clone! He dropped {0} skill points. Making new Monster...", enemy.points));
                points += enemy.points;
                hp += _random.Next(1, 6);
                enemy.Reset();
                return true;
            }
            return false;
        }

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
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "Insta-killing monster...");
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You have " + coins + " coins left.");
                        hpCheckGaben(callback, steamFriends);
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
