using System;
using System.Collections.Generic;
using System.Text;

namespace Darkhood.TexasHoldem.Core
{
    public class Player
    {
        public bool IsDealer { get; set; }
        public bool IsSmallBlind { get; set; }
        public bool IsBigBlind { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
	    public decimal Chips { get; set; }
        public List<Card> StartingHand { get; set; }
        public Hand CurrentHand { get; set; }

        public decimal CallSum { get; set; }
        public decimal CalledSum { get; set; }
        public bool Folded { get; set; }
        public bool Checked { get; set; }

        public Player(int playerId, string playerName)
        {
            this.PlayerId = playerId;
            this.PlayerName = playerName;
            this.Chips = GameSettings.GetInstance().BuyIn;
            this.StartingHand = new List<Card>(2);
            this.CurrentHand = new Hand();
        }

        public void AddCard(Card card)
        {
            this.StartingHand.Add(card);
        }

        /// <summary>
        /// Evaluates the best possible hand for the player
        /// </summary>
        /// <param name="tableCards">Array of cards from the table to be added to player's starting hand before the evaluation.</param>
        /// <returns>The player's best hand containing only 5 cards.</returns>
        public Hand GetBestPossibleHand(IEnumerable<Card> tableCards)
        {
            List<Card> cards = new List<Card>(tableCards);
            cards.AddRange(StartingHand);
            return Hand.GetBestPossibleHand(cards);
        }
    }
}
