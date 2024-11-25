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

        Player player = clientSession.CurrentPlayer;
        GameRoom room = player.Room;
        if (player == null || room == null)
            return;

        room.HandleMove(player, movePacket);
    }

    public static void HandleC_SkillPacket(PacketSession session, IMessage packet)
    {
        C_Skill skillPacket = packet as C_Skill;
        ClientSession clientSession = session as ClientSession;

        Console.WriteLine($"C_Skill: {skillPacket.SkillInfo.SkillId}");

        Player player = clientSession.CurrentPlayer;
        GameRoom room = player.Room;
        if (player == null || room == null)
            return;

        room.HandleSkill(player, skillPacket);
    }
}