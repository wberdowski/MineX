namespace MineX.Query
{
    public abstract class Stats
    {
        public string Motd { get; internal set; }
        public string GameType { get; internal set; }
        public string Map { get; internal set; }
        public int NumPlayers { get; internal set; }
        public int MaxPlayers { get; internal set; }
        public ushort HostPort { get; internal set; }
        public string HostIp { get; internal set; }
    }
}
