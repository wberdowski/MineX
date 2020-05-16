using MineX.Query;
using System;
using System.Net.Sockets;

namespace MineX.Samples.Query
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                // Create a new instance of QueryClient and set server endpoint to localhost:25565
                var queryClient = new QueryClient("localhost", 25565);

                // Send query server for basic statistics
                var basicStats = queryClient.QueryBasicStats();

                // Print basic statistics to console
                Console.WriteLine("==  BASIC STATS ==");
                foreach (var property in typeof(BasicStats).GetProperties())
                {
                    Console.WriteLine($"{property.Name.PadRight(12, ' ')} {property.GetValue(basicStats)}");
                }

                // Send query server for full statistics
                var fullStats = queryClient.QueryFullStats();

                // Print full statistics to console
                Console.WriteLine("==  FULL STATS ==");
                foreach (var property in typeof(FullStats).GetProperties())
                {
                    Console.WriteLine($"{property.Name.PadRight(12, ' ')} {property.GetValue(fullStats)}");
                }
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Query timed out.");
            }
            catch (SocketException)
            {
                Console.WriteLine("A connection with the Query server could not be established.");
            }

            // Keep console open until user presses any key
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
