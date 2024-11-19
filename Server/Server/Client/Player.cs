using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.Protocol;

namespace Server
{
    public class Player
    {
        public PlayerInfo PlayerInfo { get; set; } = new PlayerInfo() { PositionInfo = new PositionInfo() };
        public GameRoom Room { get; set; }
        public ClientSession Session { get; set; }
    }
}