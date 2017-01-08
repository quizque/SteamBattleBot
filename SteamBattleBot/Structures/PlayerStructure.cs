using System;
using SteamKit2;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace SteamBattleBot.Structures
{
    class PlayerStructure
    {
        public ulong playerId64;

        public int healthPoints;
        public string elementClass;
    }
}
