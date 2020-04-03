using MineX.Utils;
using System.IO;

namespace MineX.Rcon
{
    public class RconPacket
    {
        public int Length { get; private set; }
        public int RequestId { get; set; }
        public RconPacketType Type { get; set; }
        public byte[] Payload { get; set; }

        public RconPacket()
        {

        }

        public RconPacket(int requestId, RconPacketType type, byte[] payload)
        {
            RequestId = requestId;
            Type = type;
            Payload = payload;
        }

        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryStreamWriter(stream);
                writer.Write(Payload.Length + 10, ByteOrder.BigEndian);
                writer.Write(RequestId, ByteOrder.BigEndian);
                writer.Write((int)Type, ByteOrder.BigEndian);
                stream.Write(Payload);
                writer.Write((ushort)0, ByteOrder.BigEndian);

                return stream.ToArray();
            }
        }

        public static RconPacket Deserialize(byte[] bytes, int offset, int length)
        {
            var packet = new RconPacket();

            using (var stream = new MemoryStream(bytes, offset, length))
            {
                var reader = new BinaryStreamReader(stream);

                packet.Length = reader.ReadInt32(ByteOrder.LittleEndian);
                packet.RequestId = reader.ReadInt32(ByteOrder.LittleEndian);
                packet.Type = (RconPacketType)reader.ReadInt32(ByteOrder.LittleEndian);
                packet.Payload = new byte[length - sizeof(int) * 3 - 2];
                stream.Read(packet.Payload, 0, packet.Payload.Length);
                reader.ReadUShort(ByteOrder.LittleEndian);

                return packet;
            }
        }
    }
}
