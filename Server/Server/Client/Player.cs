using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.Protocol;

namespace Server
{
    public class Player
    {
        public PlayerInfo Info { get; set; } = new PlayerInfo();
        public GameRoom Room { get; set; }
        public ClientSession Session { get; set; }
    }
}