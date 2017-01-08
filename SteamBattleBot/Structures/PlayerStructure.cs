using System;
using SteamKit2;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace SteamBattleBot.Structures
{
    class PlayerStructure
    {
        public ulong PlayerId64;

        public int HealthPoints;
        public string Class;
    }
}
