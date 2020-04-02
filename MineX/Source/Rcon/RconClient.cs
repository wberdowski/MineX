using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MineX.Rcon
{
    /// <summary>
    /// A client implementation of the Minecraft RCON protocol.
    /// </summary>
    public class RconClient
    {
        /// <summary>
        /// Specifies the number of milliseconds after which an exception is thrown if the server does not respond.
        /// </summary>
        public int Timeout
        {
            get => timeout;
            set
            {
                timeout = value;
                socket.SendTimeout = timeout;
                socket.ReceiveTimeout = timeout;
            }
        }

        private readonly Socket socket = new Socket(SocketType.Stream, ProtocolType.IP);
        private readonly byte[] recvBuffer = new byte[1024 * 5];
        private readonly Random rand = new Random();
        private int timeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="RconClient"/>.
        /// </summary>
        public RconClient()
        {
            // Set default timeout to 10 seconds
            Timeout = 10000;
        }

        /// <summary>
        /// Establishes a connection to a Minecraft server using hostname and a port number.
        /// </summary>
        /// <param name="hostname">Server hostname or IP address.</param>
        /// <param name="port">RCON port number.</param>
        /// <exception cref="SocketException"/>
        public void Connect(string hostname, int port = 25575)
        {
            socket.Connect(hostname, port);
        }

        /// <summary>
        /// Establishes a connection to a Minecraft server using IP address and a port number.
        /// </summary>
        /// <param name="address">Server IP address.</param>
        /// <param name="port">RCON port number.</param>
        /// <exception cref="SocketException"/>
        public void Connect(IPAddress address, int port = 25575)
        {
            socket.Connect(address, port);
        }

        /// <summary>
        /// Establishes a connection to a Minecraft server using <see cref="IPEndPoint"/>.
        /// </summary>
        /// <param name="serverEp">Server endpoint.</param>
        /// <exception cref="SocketException"/>
        public void Connect(IPEndPoint serverEp)
        {
            socket.Connect(serverEp);
        }

        /// <summary>
        /// Authenticates the client with a password in order to access server commands. Requires established connection to the server.
        /// </summary>
        /// <exception cref="NotConnectedException"/>
        /// <exception cref="IncorrectPasswordException"/>
        /// <param name="passPhrase"></param>
        public void Login(string passPhrase)
        {
            if (socket.Connected)
            {
                var reqId = rand.Next();
                var request = new RconPacket(reqId, RconPacketType.SERVERDATA_AUTH, Encoding.UTF8.GetBytes(passPhrase));
                socket.Send(request.Serialize());

                var len = socket.Receive(recvBuffer);
                var response = RconPacket.Deserialize(recvBuffer, 0, len);

                if (response.RequestId == -1)
                {
                    throw new IncorrectPasswordException();
                }
            }
            else
            {
                throw new NotConnectedException("Cannot login while client is not connected to the server.");
            }
        }

        /// <summary>
        /// Sends a command to the server and returns it's output. Requires established connection to the server.
        /// </summary>
        /// <exception cref="NotConnectedException"/>
        /// <param name="command"></param>
        public string SendCommand(string command)
        {
            if (socket.Connected)
            {
                var reqId = rand.Next();
                var request = new RconPacket(reqId, RconPacketType.SERVERDATA_EXECCOMMAND, Encoding.UTF8.GetBytes(command));

                socket.Send(request.Serialize());

                var sb = new StringBuilder();
                var len = 0;
                RconPacket response = null;

                do
                {
                    len = socket.Receive(recvBuffer);
                    response = RconPacket.Deserialize(recvBuffer, 0, len);

                    sb.Append(Encoding.ASCII.GetString(response.Payload));
                } while (response.Payload.Length >= 4096);

                return sb.ToString();
            }
            else
            {
                throw new NotConnectedException("Cannot login while client is not connected to the server.");
            }
        }
    }
}
