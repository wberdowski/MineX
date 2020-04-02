namespace MineX.Query
{
    public class FullStats : Stats
    {
        public string GameId { get; internal set; }
        public string Version { get; internal set; }
        public string Plugins { get; internal set; }
        public string[] Players { get; internal set; }
    }
}
