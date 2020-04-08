using MineX.Common.Structs;
using MineX.Common.Utils;
using System.IO;

namespace MineX.Query
{
    public class StatsRequest : QueryPacket
    {
        public int ChallengeToken { get; set; }
        public StatsType StatsType { get; set; }

        public StatsRequest(Session session, int challengeToken, StatsType statsType)
        {
            Type = QueryPacketType.Stat;
            Session = session;
            ChallengeToken = challengeToken;
            StatsType = statsType;
        }

        public override byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryStreamWriter(stream);

                writer.Write(Magic, ByteOrder.LittleEndian);
                writer.Write((byte)Type);
                writer.Write(Session.Id, ByteOrder.LittleEndian);
                writer.Write(ChallengeToken, ByteOrder.LittleEndian);

                if (StatsType == StatsType.Full)
                {
                    writer.Write(0, ByteOrder.LittleEndian);
                }

                return stream.ToArray();
            }
        }
    }
}
