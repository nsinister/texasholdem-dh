using Darkhood.TexasHoldem.Net.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Darkhood.TexasHoldem.Net
{
    public class ClientEventHandler : IGameEventProcessor
    {
        public event EventHandler<EventArgs> OnCall;
        public event EventHandler<EventArgs> OnFold;
        public event EventHandler<EventArgs> OnBet;
        public event EventHandler<EventArgs> OnCheck;
        public event EventHandler<EventArgs> OnAllIn;
        public event EventHandler<EventArgs> OnRaise;
        public event EventHandler<EventArgs> OnPlayerConnect;
        public event EventHandler<EventArgs> OnPlayerDisconnect;

        public void ProcessEvent(GameEvent gameEvent)
        {
            if (gameEvent.EventType == GameEvent.GameEventType.PlayerConnected)
            {
                OnPlayerConnect?.Invoke(gameEvent, new EventArgs());
            }
            else
            {
                Console.WriteLine("DEBUG: ProcessEvent from " + gameEvent.RaisedClientId + ": "
                    + Enum.GetName(typeof(GameEvent.GameEventType), gameEvent.EventType));
            }
        }
    }
}
