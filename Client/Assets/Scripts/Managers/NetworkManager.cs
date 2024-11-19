using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Google.Protobuf;

public class NetworkManager
{
	ServerSession session = new ServerSession();

	public void Send(ArraySegment<byte> sendBuff)
	{
		session.Send(sendBuff);
	}

	public void Init()
	{
		// DNS (Domain Name System)
		string host = Dns.GetHostName();
		IPHostEntry ipHost = Dns.GetHostEntry(host);
		IPAddress ipAddr = ipHost.AddressList[0];
		IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

		Connector connector = new Connector();

		connector.Connect(endPoint,
			() => { return session; },
		1);
	}

	public void Update()
	{
		List<PacketMessage> list = PacketQueue.Instance.PopAll(); // maybe list is a null or something
		foreach (PacketMessage packet in list)
		{
			Action<PacketSession, IMessage> handler = PacketManager.Instance.GetActualPacketHandler(packet.Id);
			if (handler != null)
				handler.Invoke(session, packet.Message);
		}
	}

}
