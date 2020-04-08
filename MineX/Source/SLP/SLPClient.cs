using MineX.Common;
using MineX.Common.Structs;
using MineX.Common.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Text;

namespace MineX.Slp
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

        /// <summary>
        /// Establishes a connection to a Minecraft server using hostname and a port number. 
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <exception cref="SocketException"/>
        public void Connect(string hostname, int port)
        {
            socket.Connect(hostname, port);
        }

        public ServerStatus GetStatus()
        {
            if (socket.Connected)
            {
                // Send handshake
                PerformHandshake();

                // Send SLP request packet
                SendSlpRequest();

                byte[] recvBuffer = new byte[1024 * 128];
                int len = socket.Receive(recvBuffer);

                var packet = NotchianPacket.Deserialize(recvBuffer, 0, len);
                var jsonData = Encoding.UTF8.GetString(packet.Data, 0, packet.Data.Length);
                var token = JsonConvert.DeserializeObject<JToken>(jsonData);

                var status = new ServerStatus()
                {
                    Version = token["version"]["name"].ToString(),
                    ProtocolVersion = token["version"]["protocol"].Value<int>(),
                    Players = null,
                    MaxPlayers = token["players"]["max"].Value<int>(),
                    OnlinePlayersCount = token["players"]["online"].Value<int>(),
                    Description = token["description"]["text"].ToString(),
                    Favicon = BitmapConvert.FromBase64String(token["favicon"].ToString().Substring("data:image/png;base64,".Length))
                };
                return status;
            }
            else
            {
                throw new NotConnectedException();
            }
        }

        private void PerformHandshake()
        {
            var handshakePacket = new HandshakePacket(-1, "localhost", 25565, 1);
            var nPacket = new NotchianPacket(0x00, handshakePacket.Serialize());
            socket.Send(nPacket.Serialize());
        }

        private void SendSlpRequest()
        {
            var mcPacket2 = new NotchianPacket(0x00, new byte[0]);
            socket.Send(mcPacket2.Serialize());
        }
    }
}
