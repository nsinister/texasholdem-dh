using System;
using System.Collections.Generic;
using System.Text;

namespace Darkhood.TexasHoldem.Core
{
    public class GameSettings
    {
        public decimal BuyIn { get; set; } = 20000m;
        public decimal Stake { get; set; } = 800m;
        public decimal BlindRaiseSum { get; set; } = 400m;


        private static GameSettings _instance;

        public static GameSettings GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameSettings();
            }
            return _instance;
        }
    }
}
