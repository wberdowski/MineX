using MineX.Utils;
using System.Collections.Generic;
using System.IO;

namespace MineX.Query
{
    public class FullStatsResponse : QueryPacket
    {
        public FullStats Stats { get; protected set; }

        public static FullStatsResponse Deserialize(byte[] bytes, int offset, int length)
        {
            using (var stream = new MemoryStream(bytes, offset, length))
            {
                var response = new FullStatsResponse();
                var reader = new Utils.VariableEndianBinaryReader(stream);

                response.Type = (QueryPacketType)reader.ReadByte();
                response.Session = new Session(reader.ReadInt32(ByteOrder.LittleEndian));

                // Skip padding
                stream.Seek(11, SeekOrigin.Current);

                // Read K, V section
                Dictionary<string, string> pairs = new Dictionary<string, string>();

                while (true)
                {
                    var key = reader.ReadString();

                    if (key.Length == 0) break;

                    var value = reader.ReadString();
                    pairs.Add(key, value);
                }

                // Skip padding
                stream.Seek(10, SeekOrigin.Current);

                // Read players
                List<string> players = new List<string>();

                while (true)
                {
                    var player = reader.ReadString();
                    if (player.Length == 0) break;
                    players.Add(player);
                }

                response.Stats = new FullStats()
                {
                    Motd = pairs["hostname"],
                    GameType = pairs["gametype"],
                    GameId = pairs["game_id"],
                    Version = pairs["version"],
                    Plugins = pairs["plugins"],
                    Map = pairs["map"],
                    NumPlayers = int.Parse(pairs["numplayers"]),
                    MaxPlayers = int.Parse(pairs["maxplayers"]),
                    HostPort = ushort.Parse(pairs["hostport"]),
                    HostIp = pairs["hostip"],
                    Players = players.ToArray()
                };

                return response;
            }
        }
    }
}
