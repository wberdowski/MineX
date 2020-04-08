using MineX.Common.Structs;
using System.Drawing;

namespace MineX.Slp
{
    public class ServerStatus
    {
        public string Version { get; internal set; }
        public int ProtocolVersion { get; internal set; }
        public Player[] Players { get; internal set; }
        public int MaxPlayers { get; internal set; }
        public int OnlinePlayersCount { get; internal set; }
        public string Description { get; internal set; }
        public Bitmap Favicon { get; internal set; }
    }
}
