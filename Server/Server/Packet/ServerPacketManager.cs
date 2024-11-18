using System;
using System.Collections.Generic;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;

public class PacketManager
{
    // Singleton Pattern
    static PacketManager instance = new PacketManager();
    public static PacketManager Instance { get { return instance; } }

    Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> deserializerDict = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
    Dictionary<ushort, Action<PacketSession, IMessage>> packetHandlerDict = new Dictionary<ushort, Action<PacketSession, IMessage>>();

    public PacketManager()
    {
        Register();
    }

    public void Register()
    {
        
        deserializerDict.Add((ushort)MsgId.CMove, HandlePacket<C_Move>);
        packetHandlerDict.Add((ushort)MsgId.CMove, PacketHandler.HandleC_MovePacket);
    }

    public void ProcessPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IMessage> callback = null)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        Action<PacketSession, ArraySegment<byte>, ushort> action = null;
        if (deserializerDict.TryGetValue(id, out action))
            action.Invoke(session, buffer, id);
    }

    void HandlePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
    {
        T packet = new T();
        packet.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
        Action<PacketSession, IMessage> action = null;
        if (packetHandlerDict.TryGetValue(id, out action))
            action.Invoke(session, packet);
    }

    public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
    {
        Action<PacketSession, IMessage> action = null;
        if (packetHandlerDict.TryGetValue(id, out action))
            return action;

        return null;
    }
}
