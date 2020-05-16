using MineX.Common.Structs;
using MineX.Rcon;
using System;
using System.Net.Sockets;

namespace MineX.Samples.Rcon
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                // Create a new instance of RconClient
                var rconClient = new RconClient();

                // Connect to the server at localhost:25575
                rconClient.Connect("localhost");

                // Login with password
                rconClient.Login("password");

                // Send command and get its output
                var cmdOutput = rconClient.SendCommand("list");

                // Print command output
                Console.WriteLine(cmdOutput);
            }
            catch (IncorrectPasswordException)
            {
                Console.WriteLine("Incorrect RCON password.");
            }
            catch (SocketException)
            {
                Console.WriteLine("A connection with the RCON server could not be established.");
            }

            // Keep console open until user presses any key
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
