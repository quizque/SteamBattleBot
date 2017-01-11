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

        public ulong id;

        public int healthPoints;

        private Structures.EnemyStructure enemy = new Structures.EnemyStructure();

        public void setupGame()
        {
            enemy.hp = _random.Next(90, 120);
            healthPoints = 100;
        }
    }
}
