protoc -I=./ --csharp_out=./ Protocol.proto
IF ERRORLEVEL 1 PAUSE

START /WAIT ../../../Server/PacketGenerator/bin/PacketGenerator ./Protocol.proto

XCOPY /Y Protocol.cs "../../../Client/Assets/Scripts/Packet"
XCOPY /Y Protocol.cs "../../../Server/Server/Packet"

XCOPY /Y ClientPacketManager.cs "../../../Client/Assets/Scripts/Packet"
XCOPY /Y ServerPacketManager.cs "../../../Server/Server/Packet"

DEL /F /Q NewPacket.cs
DEL /F /Q ClientPacketManager.cs
DEL /F /Q ServerPacketManager.cs