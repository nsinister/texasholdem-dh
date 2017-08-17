using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexasHoldem.Core.Helpers;

namespace TexasHoldem.Core
{
    public class Deck
    {
        public const int CardsInDeck = 52;
        protected IList<Card> Cards;
        private int _index = 0;
        public bool IsShuffled { get; set; }

        private static Deck _unshuffledDeck = CreateDeck(false);

        private Deck()
        {
            Cards = new List<Card>(CardsInDeck);
            IsShuffled = false;
        }

        public static Deck CreateDeck(bool shuffle)
        {
            Deck deck = new Deck();
            for (int i = 2; i <= 14; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Card card = new Card((CardSuit)i, (CardValue)j);
                    deck.Cards.Add(card);
                }
            }
            if (shuffle)
            {
                deck.Cards.Shuffle();
                deck.IsShuffled = true;
            }
        }
        
        public void Reset()
        {
            _index = CardsInDeck - 1;
        }

        public Card TakeCard()
        {
            if (_index < 0)
            {
                return null;
            }
            return Cards[_index--];
        }

        public void Shuffle()
        {
            do
            {
                Cards.Shuffle();
            }
            while (_unshuffledDeck.Cards.SequenceEqual(Cards));
        }
    }
}
