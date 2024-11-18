﻿using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerSession : PacketSession
{
	public override void OnConnected(EndPoint endPoint)
	{
		Debug.Log($"서버({endPoint})와 연결 성공.");
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