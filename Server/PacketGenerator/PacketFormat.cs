namespace PacketGenerator
{
    public class PacketFormat
    {
        public static string managerFormat =
@"using System;
using System.Collections.Generic;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;

public class PacketManager
{{
    // Singleton Pattern
    static PacketManager instance = new PacketManager();
    public static PacketManager Instance {{ get {{ return instance; }} }}

    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> packetHandlerDict = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
    Dictionary<ushort, Action<PacketSession, IMessage>> actualPacketHandlerDict = new Dictionary<ushort, Action<PacketSession, IMessage>>();

    public Action<PacketSession, IMessage, ushort> CustomPacketHandler {{ get; set; }}

    public PacketManager()
    {{
        Register();
    }}

    public void Register()
    {{
        {0}
    }}

    public void ProcessPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IMessage> callback = null)
    {{
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Action<PacketSession, ArraySegment<byte>, ushort> action = null;
        if (packetHandlerDict.TryGetValue(id, out action))
            action.Invoke(session, buffer, id);
    }}

    void HandlePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
    {{
        T packet = new T();
        packet.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

        if (CustomPacketHandler != null)
        {{
            // Client Side
            CustomPacketHandler.Invoke(session, packet, id);
        }}
        else
        {{
            // Server Side
            Action<PacketSession, IMessage> action = null;
            if (actualPacketHandlerDict.TryGetValue(id, out action))
                action.Invoke(session, packet);
        }}
    }}

    public Action<PacketSession, IMessage> GetActualPacketHandler(ushort id)
    {{
        Action<PacketSession, IMessage> action = null;
        if (actualPacketHandlerDict.TryGetValue(id, out action))
            return action;

        return null;
    }}
}}
";

        public static string managerRegisterFormat = 
@"
        packetHandlerDict.Add((ushort)MsgId.{0}, HandlePacket<{1}>);
        actualPacketHandlerDict.Add((ushort)MsgId.{0}, PacketHandler.Handle{1}Packet);";

    }
}