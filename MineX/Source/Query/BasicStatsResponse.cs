using MineX.Common.Structs;
using MineX.Common.Utils;
using System.IO;

namespace MineX.Query
{
    public class BasicStatsResponse : QueryPacket
    {
        public BasicStats Stats { get; protected set; }

        public static BasicStatsResponse Deserialize(byte[] bytes, int offset, int length)
        {
            using (var stream = new MemoryStream(bytes, offset, length))
            {
                var response = new BasicStatsResponse();
                var reader = new BinaryStreamReader(stream);

                response.Type = (QueryPacketType)reader.ReadByte();
                response.Session = new Session(reader.ReadInt32(ByteOrder.LittleEndian));

                response.Stats = new BasicStats()
                {
                    Motd = reader.ReadString(),
                    GameType = reader.ReadString(),
                    Map = reader.ReadString(),
                    NumPlayers = int.Parse(reader.ReadString()),
                    MaxPlayers = int.Parse(reader.ReadString()),
                    HostPort = reader.ReadUShort(ByteOrder.LittleEndian),
                    HostIp = reader.ReadString()
                };

                return response;
            }
        }
    }
}
