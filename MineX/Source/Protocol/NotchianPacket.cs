using MineX.Common.Structs;
using MineX.Common.Utils;
using System.IO;

namespace MineX.Common
{
    public class NotchianPacket
    {
        public VarInt Length { get; internal set; }
        public VarInt PacketId { get; set; }
        public byte[] Data { get; set; }

        public NotchianPacket()
        {

        }

        public NotchianPacket(byte packetId, byte[] data)
        {
            PacketId = packetId;
            Data = data;
        }

        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryStreamWriter(stream);

                writer.WriteVarInt(Data.Length + 1);
                writer.WriteVarInt(PacketId);
                writer.WriteByteArray(Data);

                return stream.ToArray();
            }
        }

        public static NotchianPacket Deserialize(byte[] bytes, int offset, int length)
        {
            using (var stream = new MemoryStream(bytes, offset, length))
            {
                var reader = new BinaryStreamReader(stream);
                var packet = new NotchianPacket();

                packet.Length = reader.ReadVarInt();
                packet.PacketId = reader.ReadVarInt();
                reader.ReadVarInt();
                packet.Data = reader.ReadByteArray(0, packet.Length.Value - 1);

                return packet;
            }
        }
    }
}
