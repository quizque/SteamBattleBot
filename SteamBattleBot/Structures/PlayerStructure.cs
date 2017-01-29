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

        private int hp, coins, hitChance, damageDone;

        public void setupGame()
        {
            enemy.hp = _random.Next(10, 30);
            enemy.coins = _random.Next(1, 5);
            hp = 35;
            coins = 5;
        }
        
        public void attack(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (!shopMode)
            {
                hpCheck(callback, steamFriends);

                #region Miss/Hit checker
                hitChance = _random.Next(1, 4);
                if (hitChance == 3) // Missed
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You missed the monster!");
                }
                else
                {
                    damageDone = _random.Next(1, 10); // How much damage did the player do
                    enemy.hp -= damageDone;
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You hit the monster for {0} damage!", damageDone));
                }
                hitChance = _random.Next(1, 4);
                if (hitChance == 3) // Missed
                {
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "The monster missed you!");
                }
                else
                {
                    damageDone = _random.Next(1, 10); // How much damage did the monster do
                    hp -= damageDone;
                    steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("The monster hit you for {0} damage!", damageDone));
                }
                #endregion

                hpCheck(callback, steamFriends);

                state(callback, steamFriends);
            }
        }

        public void state(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            if (!shopMode)
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("\nCurrent Status\nYou:\nHP: {0}\nCoins: {1}\nMonster:\nHP: {2}", hp, coins, enemy.hp));
        }

        private bool hpCheck(SteamFriends.FriendMsgCallback callback, SteamFriends steamFriends)
        {
            // Return true if game over
            // Return false if not
            if (hp <= 0)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You died! Restarting game...");
                setupGame();
                return true;
            }else if (enemy.hp <= 0)
            {
                steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, String.Format("You killed the Monster! He droped {0} coins. Making new Monster...", enemy.coins));
                coins += enemy.coins;
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
                    "\nWelcome to the shop! You got {0} coins\n", coins) +
                    "1. Heal 5 hp: 3 coins\n" +
                    "2. Heal 10 hp: 5 coins\n" +
                    "3. Insta-kill: 10 coins\n" +
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
                    if (coins >= 5)
                    {
                        coins -= 3;
                        hp += 5;
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You healed 5 hp!");
                    }
                    break;
                #endregion

                #region Heal 10
                case "2":
                    if (coins >= 5)
                    {
                        coins -= 5;
                        hp += 10;
                        steamFriends.SendChatMessage(callback.Sender, EChatEntryType.ChatMsg, "You healed 10 hp!");
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
                        hpCheck(callback, steamFriends);
                    }
                    break;
                    #endregion
            }
        }
    }
}
