using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class PlayerManager
    {
        public static PlayerManager Instance { get; } = new PlayerManager();

        object lockObject = new object();

        Dictionary<int, Player> playerDict = new Dictionary<int, Player>();
        int playerId = 1; // TEMP

        public Player Add()
        {
            Player player = new Player();

            lock (lockObject)
            {
                player.PlayerInfo.PlayerId = playerId;
                playerDict.Add(playerId, player);
                playerId++;
            }

            return player;
        }

        public bool Remove(int playerId)
        {
            lock (lockObject)
            {
                return playerDict.Remove(playerId);
            }
        }

        public Player Find(int playerId)
        {
            lock (lockObject)
            {
                Player player = null;
                if (playerDict.TryGetValue(playerId, out player))
                {
                    return player;
                }

                return null;
            }
        }
    }
}