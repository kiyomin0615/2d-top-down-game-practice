using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class GameRoomManager
    {
        public static GameRoomManager Instance { get; } = new GameRoomManager();

        object lockObject = new object();

        Dictionary<int, GameRoom> roomDict = new Dictionary<int, GameRoom>();
        int roomId = 1;

        public GameRoom Add()
        {
            GameRoom gameRoom = new GameRoom();

            lock (lockObject)
            {
                gameRoom.RoomId = roomId;
                roomDict.Add(roomId, gameRoom);
                roomId++;
            }

            return gameRoom;
        }

        public bool Remove(int roomId)
        {
            lock (lockObject)
            {
                return roomDict.Remove(roomId)
            }
        }

        public GameRoom Find(int roomId)
        {
            lock (lockObject)
            {
                GameRoom gameRoom = null;
                if (roomDict.TryGetValue(roomId, out gameRoom))
                {
                    return gameRoom;
                }

                return null;
            }
        }
    }
}