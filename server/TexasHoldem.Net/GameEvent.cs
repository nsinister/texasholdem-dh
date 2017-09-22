namespace Darkhood.TexasHoldem.Net
{
    public class GameEvent
    {
        public enum GameEventType
        {
            Fold = 0x10,
            Bet = 0x11,
            Raise = 0x12,
            Call = 0x13,
            Check = 0x14,
            PlayerConnected = 0x50,
        }

        public GameEventType EventType { get; set; }
        public int RaisedClientId { get; set; }

        public GameEvent(GameEventType eventType, int raisedClientId)
        {
            this.EventType = eventType;
            this.RaisedClientId = raisedClientId;
        }
    }
}