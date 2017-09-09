
namespace Darkhood.TexasHoldem.Net
{
    public static class PacketHeaders
    {
        public const byte NULL = 0x0;
        public const byte OK = 0x2;
        public const byte CL_CONNECT = 0x12;
        public const byte CL_DISCONNECT = 0x13;
        public const byte S_CONNECTION_REFUSED = 0x14;
        public const byte S_PLAYER_CONNECTED = 0x15;
        public const byte S_PLAYER_CONNECTED_NAME = 0x16;
        public const byte S_PLAYER_SEAT_NUMBER = 0x22;
        public const byte S_PLAYER_DISCONNECTED = 0x17;
        public const byte CL_READY = 0x20;
        public const byte CL_HOST_START = 0x21;

        // State changes
        public const byte S_STATE_LOBBY = 0x24;
        public const byte S_STATE_GAME_STARTED = 0x25;
        public const byte S_STATE_GAME_OVER = 0x30;
        public const byte S_STATE_PREFLOP = 0x26;
        public const byte S_STATE_FLOP = 0x27;
        public const byte S_STATE_TURN = 0x28;
        public const byte S_STATE_RIVER = 0x29;

        // Player actions
        public const byte S_ACTION_REQUEST = 0x40;
        public const byte CL_FOLD = 0x41;
        public const byte CL_BET = 0x42;
        public const byte CL_RAISE = 0x43;
        public const byte CL_CHECK = 0x44;
        public const byte CL_CALL = 0x45;
        public const byte CL_ALL_IN = 0x46;
        public const byte CL_ACTION_RESPONSE = 0x50;
        public const byte S_PLAYER_FOLDED = 0x51;
        public const byte S_PLAYER_BET = 0x52;
        public const byte S_PLAYER_RAISED = 0x53;
        public const byte S_PLAYER_CHECKS = 0x54;
        public const byte S_PLAYER_CALLED = 0x55;
        public const byte S_PLAYER_ALL_IN = 0x56;

        // Game info
        public const byte S_PLAYER_LIST = 0x80;
    }
}
