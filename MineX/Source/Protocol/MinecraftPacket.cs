using MineX.Utils;
using System.IO;

namespace MineX.Protocol
{
    public class MinecraftPacket
    {
        public byte Length { get; private set; }
        public byte PacketId { get; set; }
        public byte[] Data { get; set; }

        public MinecraftPacket()
        {

        }

        public MinecraftPacket(byte packetId, byte[] data)
        {
            PacketId = packetId;
            Data = data;
        }

        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryStreamWriter(stream);

                writer.Write(Data.Length + 1);
                writer.Write(PacketId);
                stream.Write(Data);

                return stream.ToArray();
            }
        }

        public static MinecraftPacket Deserialize(byte[] bytes, int offset, int length)
        {
            using (var stream = new MemoryStream(bytes, offset, length))
            {
                var reader = new BinaryStreamReader(stream);
                var packet = new MinecraftPacket();

                packet.Length = reader.ReadByte();
                int pos = (int)stream.Position;
                packet.PacketId = reader.ReadByte();
                packet.Data = new byte[length - pos];

                return packet;
            }
        }
    }
}
