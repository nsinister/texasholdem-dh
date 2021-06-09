using Darkhood.TexasHoldem.Core;
using System.Collections.Generic;
using Xunit;

namespace Darkhood.TexasHoldem.Core.Tests
{
    public class DeckTest
    {
        [Fact]
        public void TestCreateDeck()
        {
            Deck shuffledDeck = Deck.CreateDeck(true);
            Assert.NotNull(shuffledDeck);
            Assert.True(shuffledDeck.IsShuffled);
            Assert.Equal(Deck.CardsInDeck, shuffledDeck.Cards.Count);

            Deck rawDeck = Deck.CreateDeck(false);
            Assert.NotNull(rawDeck);
            Assert.False(rawDeck.IsShuffled);
            Assert.Equal(Deck.CardsInDeck, rawDeck.Cards.Count);
        }

        [Fact]
        public void TestIsShuffled()
        {
            Deck deck = Deck.CreateDeck(false);
            Assert.False(deck.IsShuffled);
            deck.Shuffle();
            Assert.True(deck.IsShuffled);
        }

        [Fact]
        public void TestReset()
        {
            Deck deck = Deck.CreateDeck(false);

            // memorize the first taken card
            Card cardFromDeck = deck.TakeCard();
            Assert.NotNull(cardFromDeck);
            // take the rest cards
            for (int i = 0; i < Deck.CardsInDeck - 1; i++)
            {
                deck.TakeCard();
            }
            Assert.Null(deck.TakeCard());

            deck.Reset();

            Card cardFromDeckAfterReset = deck.TakeCard();
            Assert.NotNull(cardFromDeckAfterReset);
            // it should match the first card taken before reset 
            Assert.Equal(cardFromDeckAfterReset, cardFromDeck);
        }

        [Fact]
        public void TestTakenCardsAreUnique()
        {
            Deck deck = Deck.CreateDeck(true);
            List<Card> takenCards = new List<Card>();
            for (int i = 0; i < Deck.CardsInDeck; i++)
            {
                Card takenCard = deck.TakeCard();
                Assert.DoesNotContain(takenCard, takenCards);
                takenCards.Add(takenCard);
            }

            takenCards.Clear();
            deck.Reset();
            deck.Shuffle();
            for (int i = 0; i < Deck.CardsInDeck; i++)
            {
                Card takenCard = deck.TakeCard();
                Assert.DoesNotContain(takenCard, takenCards);
                takenCards.Add(takenCard);
            }
        }

        [Fact]
        public void TestTakeCard()
        {
            Deck deck = Deck.CreateDeck(false);
            Card cardFromDeck = deck.TakeCard();
            Card anotherCardFromDeck = deck.TakeCard();

            Assert.NotNull(cardFromDeck);
            Assert.NotNull(anotherCardFromDeck);

            Card expectedCard1 = deck.Cards[Deck.CardsInDeck - 1];
            Card expectedCard2 = deck.Cards[Deck.CardsInDeck - 2];

            Assert.Equal(expectedCard1, cardFromDeck);
            Assert.Equal(expectedCard2, anotherCardFromDeck);
            Assert.NotEqual(cardFromDeck, anotherCardFromDeck);

            List<Card> takenCards = new List<Card>();
            for (int i = 0; i < Deck.CardsInDeck - 2; i++)
            {
                Card takenCard = deck.TakeCard();
                Assert.NotNull(takenCard);
                Assert.DoesNotContain(takenCard, takenCards);
                takenCards.Add(takenCard);
            }
            // test if it returns null after the last card taken from the deck
            Assert.Null(deck.TakeCard());
        }

        [Fact]
        public void TestShuffle()
        {
            Deck unshuffledDeck = Deck.CreateDeck(false);
            Deck deck = Deck.CreateDeck(false);
            deck.Shuffle();
            Assert.True(deck.IsShuffled);
            Assert.NotEqual(unshuffledDeck.Cards, deck.Cards);
        }


    }
}
