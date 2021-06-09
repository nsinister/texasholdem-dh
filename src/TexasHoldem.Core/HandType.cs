using System;
using System.Collections.Generic;
using System.Text;

namespace Darkhood.TexasHoldem.Core
{
    public enum HandType
    {
        Undefined = -1,
        HighCard = 0,
        OnePair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        Straight = 4,
        Flush = 5,
        FullHouse = 6,
        FourOfAKind = 7,
        StraightFlush = 8,
    }
}
