using System;
using System.Collections.Generic;
using System.Text;

namespace Darkhood.TexasHoldem.Core
{
    public class GameStateEventArgs : EventArgs
    {
        public GameState State { get; set; }
    }

    public class PlayerActionEventArgs : EventArgs
    {
        public Player Player { get; set; }
    }    
}
