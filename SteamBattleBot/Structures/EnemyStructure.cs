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

        public void Reset()
        {
            hp = _random.Next(10, 30);
            coins = _random.Next(1, 5);
        }
    }
}
