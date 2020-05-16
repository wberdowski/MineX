using MineX.Slp;
using System;

namespace MineX.Samples.Slp
{
    internal class Program
    {
        private static void Main()
        {
            var slpClient = new SlpClient();
            slpClient.Connect("localhost", 25565);
            var status = slpClient.GetStatus();

            Console.WriteLine(status.Version);
            Console.WriteLine(status.ProtocolVersion);
            Console.WriteLine(status.MaxPlayers);
            Console.WriteLine(status.OnlinePlayersCount);
            Console.WriteLine(status.Players);
            Console.WriteLine(status.Description);

            Console.ReadKey();
        }
    }
}
