using Darkhood.TexasHoldem.Core;
using Darkhood.TexasHoldem.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Darkhood.TexasHoldem.Server
{
    internal class ClientHandler : IDisposable
    {
        public bool IsHost { get; internal set; }
        private GameServer server;
        private TcpClient client;
        private NetworkStream networkStream;
        private BinaryReader binaryReader;
        private BinaryWriter binaryWriter;
        private static int newClientId = 0;
        private int clientId;
        private byte[] clientIdBytes;
        private byte[] playerNameBytes;
        private string playerName;
        private object _lock = new object();

        public ClientHandler(GameServer gameServer, TcpClient client)
        {
            this.clientId = ++newClientId;
            this.clientIdBytes = BitConverter.GetBytes(clientId);

            Console.WriteLine("Created ClientHandler for client " + clientId);

            this.server = gameServer;
            this.client = client;
            this.networkStream = client.GetStream();
            this.binaryReader = new BinaryReader(networkStream);
            this.binaryWriter = new BinaryWriter(networkStream);
        }

        private int GetPacketSize()
        {
            return binaryReader.ReadInt32();
        }

        private byte[] GetPacketData()
        {
            int packetSize = GetPacketSize();
            // TODO: allocate once in constructor
            byte[] buffer = new byte[packetSize];
            binaryReader.Read(buffer, 0, packetSize);
            return buffer;
        }

        private string GetStringFromPacketData(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        internal void SendPacket(byte packetHeader)
        {
            SendPacket(packetHeader, null);
        }

        internal void SendPacket(byte packetHeader, byte[] packetData)
        {
            Console.WriteLine("Sending " + packetHeader + " to " + clientId);
            lock (_lock)
            {
                networkStream.WriteByte(packetHeader);
                if (packetData != null)
                {
                    int packetSize = packetData.Length;
                    binaryWriter.Write(packetSize);
                    binaryWriter.Write(packetData);
                }
            }
        }

        internal void Run()
        {
            using (var networkStream = client.GetStream())
            {
                bool done = false;
                while (!done)
                {
                    byte packetHeader = (byte)networkStream.ReadByte();
                    if (HandlePacket(packetHeader))
                    {
                        done = true;
                    }
                }
            }
        }

        private bool HandlePacket(byte packetHeader)
        {
            Console.WriteLine(packetHeader + " from " + clientId);
            switch(packetHeader)
            {
                case PacketHeaders.CL_CONNECT:
                    if (server.Game.GameState != GameState.Lobby)
                    {
                        SendPacket(PacketHeaders.S_CONNECTION_REFUSED);
                        return true;
                    }

                    playerNameBytes = GetPacketData();
                    playerName = GetStringFromPacketData(playerNameBytes);

                    SendPacket(PacketHeaders.OK);
                    //SendPacket(PacketHeaders.S_PLAYER_LIST);
                    // TODO: send player list
                    
                    

                    server.BroadcastPacket(PacketHeaders.S_PLAYER_CONNECTED, clientIdBytes);
                    server.BroadcastPacket(PacketHeaders.S_PLAYER_CONNECTED_NAME, playerNameBytes);
                    break;
                case PacketHeaders.CL_DISCONNECT:
                    server.BroadcastPacket(PacketHeaders.S_PLAYER_DISCONNECTED, clientIdBytes);
                    return true;
                case PacketHeaders.CL_HOST_START:
                    if (IsHost && server.ConnectedClientCount > 1
                        && server.Game.GameState == GameState.Lobby)
                    {
                        server.Game.GameState = GameState.GameStarted;
                        server.BroadcastPacket(PacketHeaders.S_STATE_GAME_STARTED);
                    }
                    break;
                case PacketHeaders.CL_FOLD:
                    server.BroadcastPacket(PacketHeaders.S_PLAYER_FOLDED, clientIdBytes);
                    break;
                default:
                    // FIXME temp
                    SendPacket(PacketHeaders.NULL);
                    break;
            }
            return false;
        }

        public void Dispose()
        {
            this.binaryReader?.Close();
            this.binaryWriter?.Close();
            this.networkStream?.Close();
        }
    }
}
