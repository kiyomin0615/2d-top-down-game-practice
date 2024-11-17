#!/bin/bash

protoc -I=./ --csharp_out=./ ./Protocol.proto

../../../Server/PacketGenerator/bin/PacketGenerator ./Protocol.proto

cp -f NewPacket.cs "../../../Client/Assets/Scripts/Packet"
cp -f NewPacket.cs "../../../Server/Server/Packet"

cp -f ClientPacketManager.cs "../../../Client/Assets/Scripts/Packet"
cp -f ServerPacketManager.cs "../../../Server/Server/Packet"

rm -f NewPacket.cs
rm -f ClientPacketManager.cs
rm -f ServerPacketManager.cs