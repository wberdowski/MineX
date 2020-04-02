using MineX.Utils;
using System.IO;

namespace MineX.Query
{
    public class HandshakeResponse : QueryPacket
    {
        public int ChallengeToken { get; protected set; }

        public static HandshakeResponse Deserialize(byte[] bytes, int offset, int length)
        {
            using (var stream = new MemoryStream(bytes, offset, length))
            {
                var response = new HandshakeResponse();
                var reader = new VariableEndianBinaryReader(stream);

                response.Type = (QueryPacketType)reader.ReadByte();
                response.Session = new Session(reader.ReadInt32(ByteOrder.LittleEndian));
                response.ChallengeToken = int.Parse(reader.ReadString());

                return response;
            }
        }
    }
}
