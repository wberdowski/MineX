using MineX.Common.Structs;
using MineX.Common.Utils;
using System.IO;

namespace MineX.Common
{
    public class HandshakePacket
    {
        public VarInt ProtocolVersion { get; set; }
        public string ServerAddress { get; set; }
        public ushort ServerPort { get; set; }
        public VarInt NextState { get; set; }

        public HandshakePacket()
        {

        }

        public HandshakePacket(VarInt protocolVersion, string serverAddress, ushort serverPort, VarInt nextState)
        {
            ProtocolVersion = protocolVersion;
            ServerAddress = serverAddress;
            ServerPort = serverPort;
            NextState = nextState;
        }

        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryStreamWriter(stream);

                writer.WriteVarInt(ProtocolVersion);
                writer.WriteStringRaw(ServerAddress);
                writer.WriteShort((short)ServerPort, ByteOrder.LittleEndian);
                writer.WriteVarInt(NextState);

                return stream.ToArray();
            }
        }
    }
}
