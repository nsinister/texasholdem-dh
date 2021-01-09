using Darkhood.TexasHoldem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TexasHoldemWeb.Models
{
    public class CardModel
    {
        public CardModel()
        {
        }

        public CardModel(Card card, bool isFlipped = false)
        {
            this.Value = card.Value;
            this.Suit = card.Suit;
            this.IsFlipped = isFlipped;
        }
        public CardValue Value { get; set; }
        public CardSuit Suit { get; set; }
        public bool IsFlipped { get; set; }
    }
}
