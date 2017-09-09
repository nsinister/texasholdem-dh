using Microsoft.VisualStudio.TestTools.UnitTesting;

using Darkhood.TexasHoldem.Core;

namespace Darkhood.TexasHoldem.Core.Tests
{
    [TestClass]
    public class DeckTest
    {
        [TestMethod]
        public void TestCreateDeck()
        {
            Deck shuffledDeck = Deck.CreateDeck(true);
            Assert.IsNotNull(shuffledDeck);
            Assert.IsTrue(shuffledDeck.IsShuffled);
            Assert.AreEqual(Deck.CardsInDeck, shuffledDeck.Cards.Count);

            Deck rawDeck = Deck.CreateDeck(false);
            Assert.IsNotNull(rawDeck);
            Assert.IsFalse(rawDeck.IsShuffled);
            Assert.AreEqual(Deck.CardsInDeck, rawDeck.Cards.Count);
        }

        [TestMethod]
        public void TestIsShuffled()
        {
            Deck deck = Deck.CreateDeck(false);
            Assert.IsFalse(deck.IsShuffled);
            deck.Shuffle();
            Assert.IsTrue(deck.IsShuffled);
        }

        [TestMethod]
        public void TestReset()
        {
            Deck deck = Deck.CreateDeck(false);

            // memorize the first taken card
            Card cardFromDeck = deck.TakeCard();
            Assert.IsNotNull(cardFromDeck);
            // take the rest cards
            for (int i = 0; i < Deck.CardsInDeck - 1; i++)
            {
                deck.TakeCard();
            }
            Assert.IsNull(deck.TakeCard());

            deck.Reset();

            Card cardFromDeckAfterReset = deck.TakeCard();
            Assert.IsNotNull(cardFromDeckAfterReset);
            // it should match the first card taken before reset 
            Assert.AreEqual(cardFromDeckAfterReset, cardFromDeck);
        }

        [TestMethod]
        public void TestTakeCard()
        {
            Deck deck = Deck.CreateDeck(false);
            Card cardFromDeck = deck.TakeCard();
            Card anotherCardFromDeck = deck.TakeCard();

            Assert.IsNotNull(cardFromDeck);
            Assert.IsNotNull(anotherCardFromDeck);

            Card expectedCard1 = deck.Cards[Deck.CardsInDeck - 1];
            Card expectedCard2 = deck.Cards[Deck.CardsInDeck - 2];

            Assert.AreEqual(expectedCard1, cardFromDeck);
            Assert.AreEqual(expectedCard2, anotherCardFromDeck);
            Assert.AreNotEqual(cardFromDeck, anotherCardFromDeck);

            for (int i = 0; i < Deck.CardsInDeck - 2; i++)
            {
                Card takenCard = deck.TakeCard();
                Assert.IsNotNull(takenCard);
            }
            // test if it returns null after the last card taken from the deck
            Assert.IsNull(deck.TakeCard());
        }

        [TestMethod]
        public void TestShuffle()
        {
            Deck unshuffledDeck = Deck.CreateDeck(false);
            Deck deck = Deck.CreateDeck(false);
            deck.Shuffle();
            Assert.IsTrue(deck.IsShuffled);
            Assert.AreNotEqual(unshuffledDeck.Cards, deck.Cards);
        }
    }
}
