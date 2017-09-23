using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using Darkhood.TexasHoldem.Net;
using Darkhood.TexasHoldem.Core;

namespace Darkhood.TexasHoldem.Server
{
    internal class GameServer
    {
        private IPAddress _listenAddress;
        private int _port;
        private bool _isRunning;
        private List<ClientHandler> _connectedClients = new List<ClientHandler>();
        private List<Task> _connections = new List<Task>();
        private object _lock = new object();

        internal Tournament Game;

        public const int MaxPlayers = 10;

        public int ConnectedClientCount
        {
            get
            {
                return _connectedClients.Count;
            }
        }

        public GameServer()
        {
            _listenAddress = IPAddress.Parse("127.0.0.1");
            _port = 44491;
            Game = new Tournament();
        }

        public GameServer(string ipAddress, int port)
        {
            _listenAddress = IPAddress.Parse(ipAddress);
            _port = port;
            Game = new Tournament();
        }

        public async Task Listen()
        {
            TcpListener listener = new TcpListener(_listenAddress, _port);
            try
            {
                listener.Start();
                _isRunning = true;

                Console.WriteLine("Listening on " + _port);

                while (_isRunning)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("Accepted client");
                    var task = StartHandleConnectionAsync(client);
                    if (task.IsFaulted)
                    {
                        task.Wait();
                    }                    
                }
            }
            catch (Exception ex)
            {
                // TODO Log
                Console.WriteLine(ex.ToString());
                listener.Stop();
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            await Task.Yield();

            using (var clientHandler = new ClientHandler(this, client))
            {
                if (_connectedClients.Count >= MaxPlayers)
                {
                    clientHandler.SendPacket(PacketHeaders.S_CONNECTION_REFUSED);
                }
                else
                {
                    _connectedClients.Add(clientHandler);
                    clientHandler.Run();
                }
            }
        }

        private async Task StartHandleConnectionAsync(TcpClient client)
        {
            var handleClientTask = HandleClient(client);

            lock (_lock)
            {
                _connections.Add(handleClientTask);
            }

            try
            {
                await handleClientTask;
            }
            catch (Exception ex)
            {
                // TODO Log
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                lock (_lock)
                {
                    _connections.Remove(handleClientTask);
                }
            }
        }

        internal void BroadcastPacket(byte packetHeader)
        {
            BroadcastPacket(packetHeader, null);
        }

        internal void BroadcastPacket(byte packetHeader, byte[] packetData)
        {
            foreach (ClientHandler client in _connectedClients)
            {
                client.SendPacket(packetHeader, packetData);
            }
        }
    }
}
