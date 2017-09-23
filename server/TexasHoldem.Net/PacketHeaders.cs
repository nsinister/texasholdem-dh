
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

        public const byte S_GAME_LIST_BEGIN = 0x81;
        public const byte S_GAME_LIST_END = 0x82;

        // Open lobby item, followed by name, session ID,
        // number of players available and max players
        public const byte S_GAME_LOBBY_ITEM = 0x83;

        // When client sends CL_CREATE_LOBBY and the server successfully creates lobby,
        // it will broadcast a signal to all clients to refresh their lobby list
        public const byte S_LOBBY_CREATED = 0x84;

        // Create lobby. Requires game name. Server responds with newly created session ID. 
        // Client becomes its host.
        public const byte CL_CREATE_LOBBY = 0x70;

        // Join lobby with the specified session ID
        public const byte CL_JOIN_LOBBY = 0x71;
    }
}
