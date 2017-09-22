using System;
using System.Collections.Generic;
using System.Text;

namespace Darkhood.TexasHoldem.Net.Events
{
    public class PlayerConnectedEvent : GameEvent
    {
        public string PlayerName { get; set; }
        //public int SeatNumber { get; set; }

        public PlayerConnectedEvent(int fromClientId, string connectedPlayerName)
            : base(GameEventType.PlayerConnected, fromClientId)
        {
            this.PlayerName = connectedPlayerName;
            //this.SeatNumber = seatNumber;
        }
    }
}
