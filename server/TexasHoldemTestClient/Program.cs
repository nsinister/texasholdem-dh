using System;
using System.Net.Sockets;
using Darkhood.TexasHoldem.Net;
using System.Threading.Tasks;
using Darkhood.TexasHoldem.Net.Events;

namespace Darkhood.TexasHoldem.TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started test client");

            ClientEventHandler eventProcessor = new ClientEventHandler();
            eventProcessor.OnPlayerConnect += EventProcessor_OnPlayerConnect;

            GameClient client = new GameClient(eventProcessor);
            client.Connect("127.0.0.1", 44491);
            var task = client.RunAsync();

            string cmd = Console.ReadLine();
            while (cmd != "exit")
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("GameClient failure");
                    if (task.Exception != null)
                    {
                        Console.WriteLine(task.Exception);
                    }
                    break;
                }

                if (cmd == "fold")
                {
                    var foldEvent = new GameEvent(GameEvent.GameEventType.Fold, 1);
                    client.SendEvent(foldEvent);
                }
                cmd = Console.ReadLine();
            }
        }

        private static void EventProcessor_OnPlayerConnect(object sender, EventArgs e)
        {
            var playerConnectedEvent = (PlayerConnectedEvent)sender;
            Console.WriteLine("FROM EVENT Connected player: " + playerConnectedEvent.PlayerName
                + " with ID " + playerConnectedEvent.RaisedClientId);
        }
    }
}
