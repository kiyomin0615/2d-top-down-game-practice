using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;

public class ServerSession : PacketSession
{
	public void Send(IMessage packet)
	{
		string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
		MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

		ushort size = (ushort)packet.CalculateSize();
		byte[] sendBuffer = new byte[size + 4];
		Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
		Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
		Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

		Send(new ArraySegment<byte>(sendBuffer));
	}

	public override void OnConnected(EndPoint endPoint)
	{
		Debug.Log($"서버({endPoint})와 연결 성공.");

		// You can manipulate Unity on main thread only
		PacketManager.Instance.CustomPacketHandler = (session, message, id) =>
		{
			PacketQueue.Instance.Push(id, message);
		};
	}

	public override void OnDisconnected(EndPoint endPoint)
	{
		Debug.Log($"서버({endPoint})와 연결 종료.");
	}

	public override void OnPacketReceived(ArraySegment<byte> buffer)
	{
		PacketManager.Instance.ProcessPacket(this, buffer);
	}

	public override void OnSent(int numOfBytes)
	{

	}
}