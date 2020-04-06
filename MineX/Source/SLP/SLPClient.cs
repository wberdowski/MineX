using MineX.Protocol;
using MineX.Utils;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace MineX.SLP
{
    /// <summary>
    /// A client implementation of the Minecraft SLP (Server List Ping) protocol.
    /// </summary>
    public class SlpClient
    {
        private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.IP);

        public SlpClient()
        {

        }

        public void GetStatus()
        {
            socket.Connect("localhost", 25565);

            using (var stream = new MemoryStream())
            {
                var writer = new BinaryStreamWriter(stream);
                writer.Write(578, ByteOrder.BigEndian);
                writer.Write("localhost", Encoding.UTF8);
                writer.Write((ushort)25565, ByteOrder.LittleEndian);
                writer.Write(0x01, ByteOrder.BigEndian);

                var packet = new MinecraftPacket(0x00, stream.ToArray());
                socket.Send(packet.Serialize());
            }

            //var packet2 = new MinecraftPacket(0, new byte[0]);
            //socket.Send(packet2.Serialize());

            byte[] recvBuffer = new byte[1024 * 4];
            int len = socket.Receive(recvBuffer);

            //using (var stream = new MemoryStream())
            //{
            //    var writer = new BinaryStreamWriter(stream);
            //    writer.Write(0, ByteOrder.BigEndian);
            //    writer.Write(1, ByteOrder.BigEndian);

            //    var packet = new MinecraftPacket(0x01, stream.ToArray());
            //    socket.Send(packet.Serialize());
            //}

            //byte[] recvBuffer = new byte[1024 * 4];
            //int len = socket.Receive(recvBuffer);

            Console.WriteLine("Recv {0} len", Encoding.UTF8.GetString(recvBuffer, 0, len));

            Console.WriteLine("OK");
        }
    }
}
