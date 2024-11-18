using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
	public static void HandleS_EnterGamePacket(PacketSession session, IMessage packet)
	{
		S_EnterGame enterPacket = packet as S_EnterGame;
		ServerSession serverSession = session as ServerSession;

		Debug.Log("S_EnterGame Packet has arrived.");
		Debug.Log(enterPacket.Player);
	}
	public static void HandleS_ExitGamePacket(PacketSession session, IMessage packet)
	{
		S_ExitGame exitPacket = packet as S_ExitGame;
		ServerSession serverSession = session as ServerSession;

		Debug.Log("S_ExitGame Packet has arrived.");
	}
	public static void HandleS_SpawnPacket(PacketSession session, IMessage packet)
	{
		S_Spawn spawnPacket = packet as S_Spawn;
		ServerSession serverSession = session as ServerSession;

		Debug.Log("S_Spawn Packet has arrived.");
		Debug.Log(spawnPacket.Players);
	}
	public static void HandleS_DespawnPacket(PacketSession session, IMessage packet)
	{
		S_Despawn despawnPacket = packet as S_Despawn;
		ServerSession serverSession = session as ServerSession;

		Debug.Log("S_Despawn Packet has arrived.");
	}
	public static void HandleS_MovePacket(PacketSession session, IMessage packet)
	{
		S_Move movePacket = packet as S_Move;
		ServerSession serverSession = session as ServerSession;

		Debug.Log("S_Move Packet has arrived.");
	}
}
