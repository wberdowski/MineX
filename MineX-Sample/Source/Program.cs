using MineX.Query;
using MineX.Rcon;
using System;
using System.Net.Sockets;

namespace MineXSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RunRcon();
            RunQuery();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void RunRcon()
        {
            var rconClient = new RconClient();

            try
            {
                rconClient.Connect("localhost");

                rconClient.Login("enter_password_here");

                var response = rconClient.SendCommand("rules");
                Console.WriteLine(response);
            }
            catch (IncorrectPasswordException)
            {
                Console.WriteLine("Incorrect RCON password.");
            }
            catch (SocketException)
            {
                Console.WriteLine("A connection with the RCON server could not be established.");
            }
        }

        private static void RunQuery()
        {
            var queryClient = new QueryClient("localhost", 25565);

            try
            {
                var basicStats = queryClient.QueryBasicStats();

                Console.WriteLine(
                    "==  BASIC STATS ==" + Environment.NewLine +
                    $"MOTD:\t\t{basicStats.Motd}" + Environment.NewLine +
                    $"Game type:\t{basicStats.GameType}" + Environment.NewLine +
                    $"Map:\t\t{basicStats.Map}" + Environment.NewLine +
                    $"Online players:\t{basicStats.NumPlayers}" + Environment.NewLine +
                    $"Max players:\t{basicStats.MaxPlayers}" + Environment.NewLine +
                    $"Host port:\t{basicStats.HostPort}" + Environment.NewLine +
                    $"Host IP:\t{basicStats.HostIp}" + Environment.NewLine
                    );

                var fullStats = queryClient.QueryFullStats();

                Console.WriteLine(
                    "==  FULL STATS ==" + Environment.NewLine +
                    $"MOTD:\t\t{fullStats.Motd}" + Environment.NewLine +
                    $"Game type:\t{fullStats.GameType}" + Environment.NewLine +
                    $"Game ID:\t{fullStats.GameId}" + Environment.NewLine +
                    $"Version:\t{fullStats.Version}" + Environment.NewLine +
                    $"Plugins:\t{fullStats.Plugins}" + Environment.NewLine +
                    $"Map:\t\t{fullStats.Map}" + Environment.NewLine +
                    $"Online players:\t{fullStats.NumPlayers}" + Environment.NewLine +
                    $"Max players:\t{fullStats.MaxPlayers}" + Environment.NewLine +
                    $"Host port:\t{fullStats.HostPort}" + Environment.NewLine +
                    $"Host IP:\t{fullStats.HostIp}" + Environment.NewLine +
                    $"Players:\t{string.Join(", ", fullStats.Players)}"
                    );
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Query timed out.");
            }
            catch (SocketException)
            {
                Console.WriteLine("A connection with the Query server could not be established.");
            }
        }
    }
}
