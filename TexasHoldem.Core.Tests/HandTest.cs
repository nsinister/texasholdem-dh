using Microsoft.VisualStudio.TestTools.UnitTesting;

using Darkhood.TexasHoldem.Core;
using System.Collections.Generic;

namespace Darkhood.TexasHoldem.Core.Tests
{
    [TestClass]
    public class HandTest
    {
        [TestMethod]
        public void TestGetAllPossibleHands()
        {
            List<Card> cards = new List<Card>();
            cards.Add(new Card(CardValue.Queen, CardSuit.Spades));
            cards.Add(new Card(CardValue.King, CardSuit.Spades));
            cards.Add(new Card(CardValue.Ace, CardSuit.Spades));
            cards.Add(new Card(CardValue.Seven, CardSuit.Spades));
            cards.Add(new Card(CardValue.Two, CardSuit.Spades));
            cards.Add(new Card(CardValue.Five, CardSuit.Spades));
            cards.Add(new Card(CardValue.Jack, CardSuit.Spades));

            List<Hand> allHands = Hand.GetAllPossibleHands(cards);
            Assert.AreEqual(allHands.Count, (cards.Count - 1) * cards.Count);
            // TODO: implement content check of allHands array
        }
    }
}
