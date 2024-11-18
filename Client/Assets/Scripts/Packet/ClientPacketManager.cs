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
        
        deserializerDict.Add((ushort)MsgId.SEnterGame, HandlePacket<S_EnterGame>);
        packetHandlerDict.Add((ushort)MsgId.SEnterGame, PacketHandler.HandleS_EnterGamePacket);
        deserializerDict.Add((ushort)MsgId.SExitGame, HandlePacket<S_ExitGame>);
        packetHandlerDict.Add((ushort)MsgId.SExitGame, PacketHandler.HandleS_ExitGamePacket);
        deserializerDict.Add((ushort)MsgId.SSpawn, HandlePacket<S_Spawn>);
        packetHandlerDict.Add((ushort)MsgId.SSpawn, PacketHandler.HandleS_SpawnPacket);
        deserializerDict.Add((ushort)MsgId.SDespawn, HandlePacket<S_Despawn>);
        packetHandlerDict.Add((ushort)MsgId.SDespawn, PacketHandler.HandleS_DespawnPacket);
        deserializerDict.Add((ushort)MsgId.SMove, HandlePacket<S_Move>);
        packetHandlerDict.Add((ushort)MsgId.SMove, PacketHandler.HandleS_MovePacket);
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
