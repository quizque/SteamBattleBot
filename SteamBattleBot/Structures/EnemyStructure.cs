using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamBattleBot.Structures
{
    class EnemyStructure
    {
        private Random _random = new Random();
        public int hp;
        public int coins;
        public int points;
        public int hpboss;

        public void Reset()
        {
            hp = _random.Next(10, 50);
            coins = _random.Next(1, 5);
            points = _random.Next(1, 2);
            hpboss = _random.Next(40, 50);
        }
    }
}
