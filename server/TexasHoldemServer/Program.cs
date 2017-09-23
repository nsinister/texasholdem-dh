using System;

namespace Darkhood.TexasHoldem.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer server = new GameServer("0.0.0.0", 44491);
            server.Listen().Wait();
        }
    }
}
