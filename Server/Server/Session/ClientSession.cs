using System.Net;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;

namespace Server
{
    public class ClientSession : PacketSession
    {
        public int SessionId { get; set; }

        public Player CurrentPlayer { get; set; }

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
            Console.WriteLine($"클라이언트({endPoint})와 연결 성공.");

            CurrentPlayer = PlayerManager.Instance.Add();
            CurrentPlayer.Info.Name = $"PLAYER_{CurrentPlayer.Info.PlayerId}";
            CurrentPlayer.Info.PosX = 0;
            CurrentPlayer.Info.PosY = 0;
            CurrentPlayer.Session = this;

            GameRoomManager.Instance.Find(1).EnterGame(CurrentPlayer);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            GameRoomManager.Instance.Find(1).ExitGame(CurrentPlayer.Info.PlayerId);

            SessionManager.Instance.Remove(this);

            Console.WriteLine($"클라이언트({endPoint})와 연결 종료.");
        }

        public override void OnPacketReceived(ArraySegment<byte> buffer)
        {
            PacketManager.Instance.ProcessPacket(this, buffer);
        }

        public override void OnSent(int numOfBytes)
        {

        }
    }
}