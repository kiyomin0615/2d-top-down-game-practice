using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Protocol;

namespace Server
{
    public class GameRoom
    {
        object lockObject = new object();

        public int RoomId { get; set; }

        List<Player> players = new List<Player>();

        public void EnterGame(Player newPlayer)
        {
            if (newPlayer == null)
                return;

            lock (lockObject)
            {
                players.Add(newPlayer);
                newPlayer.Room = this;

                // Server to the new player
                {
                    S_EnterGame enterPacket = new S_EnterGame();
                    enterPacket.PlayerInfo = newPlayer.PlayerInfo;
                    newPlayer.Session.Send(enterPacket);

                    S_Spawn spawnPacket = new S_Spawn();
                    foreach (Player p in players)
                    {
                        if (newPlayer != p)
                        {
                            spawnPacket.PlayerInfos.Add(p.PlayerInfo);
                        }
                    }
                    newPlayer.Session.Send(spawnPacket);
                }

                // Server to other players
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    spawnPacket.PlayerInfos.Add(newPlayer.PlayerInfo);
                    foreach (Player p in players)
                    {
                        if (newPlayer != p)
                        {
                            p.Session.Send(spawnPacket);
                        }
                    }
                }
            }
        }

        public void ExitGame(int playerId)
        {
            lock (lockObject)
            {
                Player player = players.Find(p => p.PlayerInfo.PlayerId == playerId);
                if (player == null)
                    return;

                players.Remove(player);
                player.Room = null;

                // Server to the player
                {
                    S_ExitGame exitPacket = new S_ExitGame();
                    player.Session.Send(exitPacket);
                }

                // Server to other players
                {
                    S_Despawn despawnPacket = new S_Despawn();
                    despawnPacket.PlayerIds.Add(playerId);
                    foreach (Player p in players)
                    {
                        if (player != p)
                        {
                            p.Session.Send(despawnPacket);
                        }
                    }
                }
            }
        }

        public void Broadcast(IMessage packet)
        {
            lock (lockObject)
            {
                foreach (Player p in players)
                {
                    p.Session.Send(packet);
                }
            }
        }
    }
}