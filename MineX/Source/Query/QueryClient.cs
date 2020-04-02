using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace MineX.Query
{
    /// <summary>
    /// A client implementation of the Minecraft Query protocol.
    /// </summary>
    public class QueryClient
    {
        /// <summary>
        /// Represents the server endpoint as an IP address and a port number.
        /// </summary>
        public IPEndPoint ServerEndpoint { get => (IPEndPoint)serverEp; set => serverEp = value; }
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

        private EndPoint serverEp;
        private int timeout;
        private readonly Socket socket = new Socket(SocketType.Dgram, ProtocolType.IP);
        private readonly byte[] recvBuffer = new byte[1024 * 4];
        private readonly Random rand = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryClient"/>.
        /// </summary>
        public QueryClient()
        {
            // Set default timeout to 10 seconds
            Timeout = 10000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryClient"/> with the specified server endpoint.
        /// </summary>
        /// <param name="serverEp">Server endpoint.</param>
        public QueryClient(IPEndPoint serverEp) : this()
        {
            ServerEndpoint = serverEp;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryClient"/> with the specified hostname and port number.
        /// </summary>
        /// <param name="hostname">Server hostname or IP address.</param>
        /// <param name="port">Server port number.</param>
        public QueryClient(string hostname, int port) : this()
        {
            var address = Dns.GetHostAddresses(hostname).First();
            ServerEndpoint = new IPEndPoint(address, port);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryClient"/> with the specified <see cref="IPAddress"/> and port number.
        /// </summary>
        /// <param name="address">Server IP address.</param>
        /// <param name="port">Server port number.</param>
        public QueryClient(IPAddress address, int port) : this()
        {
            ServerEndpoint = new IPEndPoint(address, port);
        }

        /// <summary>
        /// Queries the server for basic statistics.
        /// </summary>
        /// <exception cref="TimeoutException"/>
        /// <exception cref="SocketException"/>
        public BasicStats QueryBasicStats()
        {
            // Generate random session id
            var session = new Session(rand.Next());

            try
            {
                // Perform handshake
                var hsResponse = PerformHandshake(ServerEndpoint, session);

                // Send basic stats request
                var statRequest = new StatsRequest(session, hsResponse.ChallengeToken, StatsType.Basic);
                socket.SendTo(statRequest.Serialize(), ServerEndpoint);

                // Receive basic stats response
                var len = socket.ReceiveFrom(recvBuffer, ref serverEp);
                var statResponse = BasicStatsResponse.Deserialize(recvBuffer, 0, len);

                return statResponse.Stats;
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                    throw new TimeoutException();

                throw ex;
            }
        }

        /// <summary>
        /// Queries the server for full statistics.
        /// </summary>
        /// <exception cref="TimeoutException"/>
        /// <exception cref="SocketException"/>
        public FullStats QueryFullStats()
        {
            // Generate random session id
            var session = new Session(rand.Next());

            try
            {
                // Perform handshake
                var hsResponse = PerformHandshake(ServerEndpoint, session);

                // Send basic stats request
                var statRequest = new StatsRequest(session, hsResponse.ChallengeToken, StatsType.Full);
                socket.SendTo(statRequest.Serialize(), ServerEndpoint);

                // Receive basic stats response
                var len = socket.ReceiveFrom(recvBuffer, ref serverEp);
                var statResponse = FullStatsResponse.Deserialize(recvBuffer, 0, len);

                return statResponse.Stats;
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.TimedOut)
                    throw new TimeoutException();

                throw ex;
            }
        }

        /// <summary>
        /// Performs a handshake between the Minecraft server and the <see cref="QueryClient"/>.
        /// </summary>
        /// <param name="serverEp">Server enpoint.</param>
        /// <param name="session">Query session.</param>
        private HandshakeResponse PerformHandshake(EndPoint serverEp, Session session)
        {
            // Send handshake request
            var hsRequest = new HandshakeRequest(session);
            socket.SendTo(hsRequest.Serialize(), serverEp);

            // Receive handshake response
            var len = socket.ReceiveFrom(recvBuffer, ref serverEp);
            return HandshakeResponse.Deserialize(recvBuffer, 0, len);
        }
    }
}