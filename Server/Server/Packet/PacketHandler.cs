using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using ServerCore;

public class PacketHandler
{
    public static void HandleC_MovePacket(PacketSession session, IMessage packet)
    {
        C_Move movePacket = packet as C_Move;
        ClientSession clientSession = session as ClientSession;

        Console.WriteLine($"C_Move: ({movePacket.PositionInfo.PosX}, {movePacket.PositionInfo.PosX})");

        if (clientSession.CurrentPlayer == null || clientSession.CurrentPlayer.Room == null)
            return;

        // TODO: validate packets

        PlayerInfo playerInfo = clientSession.CurrentPlayer.PlayerInfo;
        playerInfo.PositionInfo = movePacket.PositionInfo;

        S_Move resMovePacket = new S_Move();
        resMovePacket.PlayerId = clientSession.CurrentPlayer.PlayerInfo.PlayerId;
        resMovePacket.PositionInfo = movePacket.PositionInfo;

        clientSession.CurrentPlayer.Room.Broadcast(resMovePacket);
    }
}