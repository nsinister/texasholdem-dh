using System;

namespace Darkhood.TexasHoldem.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer server = new GameServer();
            server.Listen().Wait();
        }
    }
}
