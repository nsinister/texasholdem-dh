using Darkhood.TexasHoldem.Net.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Darkhood.TexasHoldem.Net.GameEvent;

namespace Darkhood.TexasHoldem.Net
{
    public sealed class GameClient
    {
        public bool Connected { get; private set; }
        private TcpClient client;
        private NetworkStream stream;
        private BinaryWriter writer;
        private BinaryReader reader;
        private IGameEventProcessor eventProcessor;
        private object _lock = new object();

        public GameClient(IGameEventProcessor eventProcessor)
        {
            this.eventProcessor = eventProcessor;
        }

        public void Connect(string hostname, int port)
        {
            Random rand = new Random();
            string playerName = "Player" + rand.Next(1000);
            byte[] playerNameBytes = Encoding.UTF8.GetBytes(playerName);

            client = new TcpClient(hostname, port);
            stream = client.GetStream();
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);

            stream.WriteByte(PacketHeaders.CL_CONNECT);
            writer.Write(playerNameBytes.Length);
            writer.Write(playerNameBytes);

            byte resp = (byte)stream.ReadByte();
            if (resp == PacketHeaders.OK)
            {
                // TODO: event manager
                Console.WriteLine("Connected");
            }
        }

        public void SendEvent(GameEvent gameEvent)
        {
            byte packetHeader = PacketHeaders.NULL;
            //byte[] packetData = null;
            switch (gameEvent.EventType)
            {
                case GameEventType.Fold:
                    packetHeader = PacketHeaders.CL_FOLD;
                    break;
                default:
                    break;
            }
            if (packetHeader != PacketHeaders.NULL)
            {
                this.SendPacket(packetHeader);
            }
        }

        public Task RunAsync()
        {
            return Task.Run(() => 
            {
                bool done = false;
                while (!done)
                {
                    done = RecieveData();
                }
            });
        }

        private bool RecieveData()
        {
            byte resp = (byte)stream.ReadByte();
            if (HandlePacket(resp))
            {
                return true;
            }
            return false;
        }

        private int GetPacketSize()
        {
            return reader.ReadInt32();
        }

        private byte[] GetPacketData()
        {
            int packetSize = GetPacketSize();
            // TODO: allocate once in constructor
            byte[] buffer = new byte[packetSize];
            reader.Read(buffer, 0, packetSize);
            return buffer;
        }

        private string GetStringFromPacketData(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        private int GetIntFromPacketData(byte[] packetData)
        {
            return BitConverter.ToInt32(packetData, 0);
        }

        public void SendPacket(byte packetHeader)
        {
            SendPacket(packetHeader, null);
        }

        public void SendPacket(byte packetHeader, byte[] packetData)
        {
            lock (_lock)
            {
                stream.WriteByte(packetHeader);
                if (packetData != null)
                {
                    int packetSize = packetData.Length;
                    writer.Write(packetSize);
                    writer.Write(packetData);
                }
            }
        }

        public bool HandlePacket(byte packetHeader)
        {
            byte[] packetData;
            if (packetHeader == PacketHeaders.S_PLAYER_CONNECTED)
            {
                packetData = GetPacketData();
                int connectedClientId = GetIntFromPacketData(packetData);
                packetHeader = (byte)stream.ReadByte();
                if (packetHeader == PacketHeaders.S_PLAYER_CONNECTED_NAME)
                {
                    packetData = GetPacketData();
                    string connectedPlayerName = GetStringFromPacketData(packetData);

                    var connectedEvent = new PlayerConnectedEvent(connectedClientId, connectedPlayerName);
                    eventProcessor.ProcessEvent(connectedEvent);
                }
            }
            else if (packetHeader == PacketHeaders.S_PLAYER_FOLDED)
            {
                packetData = GetPacketData();
                int foldedClientId = GetIntFromPacketData(packetData);

                Console.WriteLine("Folded clientId " + foldedClientId);
            }
            return false;
        }
    }
}
