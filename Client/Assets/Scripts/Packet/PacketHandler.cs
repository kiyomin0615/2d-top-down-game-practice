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
		Manager.Object.Add(enterPacket.PlayerInfo, isMyPlayer: true);
	}

	public static void HandleS_ExitGamePacket(PacketSession session, IMessage packet)
	{
		S_ExitGame exitPacket = packet as S_ExitGame;
		Manager.Object.RemoveMyPlayer();
	}

	public static void HandleS_SpawnPacket(PacketSession session, IMessage packet)
	{
		S_Spawn spawnPacket = packet as S_Spawn;

		foreach (PlayerInfo playerInfo in spawnPacket.PlayerInfos)
		{
			Manager.Object.Add(playerInfo, isMyPlayer: false);
		}
	}

	public static void HandleS_DespawnPacket(PacketSession session, IMessage packet)
	{
		S_Despawn despawnPacket = packet as S_Despawn;

		foreach (int playerId in despawnPacket.PlayerIds)
		{
			Manager.Object.Remove(playerId);
		}
	}

	public static void HandleS_MovePacket(PacketSession session, IMessage packet)
	{
		S_Move movePacket = packet as S_Move;
		Debug.Log("S_Move Packet has arrived.");
	}
}
