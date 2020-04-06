using MineX.SLP;
using System;

namespace MineX.Samples.SLP
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var slpClient = new SlpClient();
            slpClient.GetStatus();

            Console.ReadKey();
        }
    }
}
