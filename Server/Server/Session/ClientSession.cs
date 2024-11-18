using System.Net;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using static Google.Protobuf.Protocol.Person.Types;

namespace Server
{
    public class ClientSession : PacketSession
    {
        public int SessionId { get; set; }

        public void Send(IMessage packet)
        {
            string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            MsgId msgId = (MsgId)Enum.Parse(typeof(MsgId), msgName);

            ushort size = (ushort)packet.CalculateSize();
            byte[] sendBuffer = new byte[size + 4];
            Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes((ushort)msgId), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

            Send(new ArraySegment<byte>(sendBuffer));
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"클라이언트({endPoint})와 연결 성공.");

            S_Chat chat = new S_Chat()
            {
                Context = "Hello!"
            };

            this.Send(chat);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
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