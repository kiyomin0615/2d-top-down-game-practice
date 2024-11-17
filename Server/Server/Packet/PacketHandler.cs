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
    public static void HandleC_ChatPacket(PacketSession session, IMessage packet)
    {
        S_Chat chatPacket = packet as S_Chat;
        ClientSession clientSession = session as ClientSession;

        Console.WriteLine(chatPacket.Context);
    }
}