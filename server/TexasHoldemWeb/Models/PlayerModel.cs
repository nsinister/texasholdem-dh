using Darkhood.TexasHoldem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TexasHoldemWeb.Models
{
    public class PlayerModel
    {
        public void UpdateFrom(Player player, bool updateCards = false, bool cardsFlipped = false)
        {
            this.PlayerId = player.PlayerId;
            this.PlayerName = player.PlayerName;
            this.IsBigBlind = player.IsBigBlind;
            this.IsDealer = player.IsDealer;
            this.IsSmallBlind = player.IsSmallBlind;
            this.Chips = player.Chips;
            if (updateCards)
            {
                this.FirstCard = new CardModel(player.StartingHand[0], cardsFlipped);
                this.SecondCard = new CardModel(player.StartingHand[1], cardsFlipped);
            }
            else
            {
                // Fake card
                this.FirstCard = new CardModel { Suit = CardSuit.Clubs, Value = CardValue.Ace };
                this.SecondCard = new CardModel { Suit = CardSuit.Clubs, Value = CardValue.Ace };
            }
        }

        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public CardModel FirstCard { get; set; }
        public CardModel SecondCard { get; set; }
        public decimal Chips { get; set; }
        public bool IsDealer { get; set; }
        public bool IsSmallBlind { get; set; }
        public bool IsBigBlind { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}
