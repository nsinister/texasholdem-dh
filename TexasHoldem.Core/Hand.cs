using System;
using System.Collections.Generic;
using System.Text;

namespace Darkhood.TexasHoldem.Core
{
    public class Hand : IComparable<Hand>
    {
        public const int MaxCardsInHand = 5;
        protected List<Card> Cards;
        private HandType _handType = HandType.Undefined;
        private IDictionary<int, int> _handHistogram;

        public Hand()
        {
            Cards = new List<Card>(MaxCardsInHand);
            _handHistogram = new Dictionary<int, int>(5);
        }

        public Hand(IEnumerable<Card> cards)
        {
            Cards = new List<Card>(cards);
            CreateCardHistogram(cards);
        }

        public void AddCard(Card card)
        {
            if (Cards.Count >= MaxCardsInHand)
            {
                throw new HandFullException("Reached maximum number of cards in hand: " + MaxCardsInHand);
            }
            _handType = HandType.Undefined;
            AddToHistogram(card);
            Cards.Add(card);
        }

        public void AddCards(IEnumerable<Card> cards)
        {
            foreach (Card c in cards)
            {
                AddCard(c);
            }
        }

        /// <summary>
        /// Get list of cards of the hand
        /// </summary>
        /// <returns>A copy of the original array of the hand cards.</returns>
        public List<Card> GetCards()
        {
            return new List<Card>(Cards);
        }

        public void Clear()
        {
            Cards.Clear();
            _handHistogram.Clear();
            _handType = HandType.Undefined;
        }

        public HandType GetHandType()
        {
            if (Cards.Count < MaxCardsInHand)
            {
                return HandType.Undefined;
            }
            if (_handType == HandType.Undefined)
            {
                int[] values = new int[_handHistogram.Values.Count];
                _handHistogram.Values.CopyTo(values, 0);
                Array.Sort(values);
                int maxRankCount = values[values.Length - 1];

                // Check if the hand is quads or boat
                if (_handHistogram.Count < 3)
                {
                    if (maxRankCount == 4)
                    {
                        return HandType.FourOfAKind;
                    }
                    else
                    {
                        return HandType.FullHouse;
                    }
                }
                else if (_handHistogram.Count == 3)
                {
                    if (maxRankCount == 3)
                    {
                        return HandType.ThreeOfAKind;
                    }
                    else
                    {
                        return HandType.TwoPair;
                    }
                }
                else if (_handHistogram.Count == 4)
                {
                    return HandType.OnePair;
                }

                bool isFlush = false;
                bool isStraight = false;
                // Check if the hand is flush
                CardSuit firstCardSuit = Cards[0].Suit;
                int sameSuitCount = 0;
                foreach (Card card in Cards)
                {
                    if (card.Suit == firstCardSuit)
                    {
                        sameSuitCount++;
                    }
                }
                isFlush = (sameSuitCount == Cards.Count);

                Card[] cardsArray = new Card[Cards.Count];
                cardsArray.CopyTo(cardsArray, 0);
                Array.Sort(cardsArray);

                // Check if the hand is straight
                CardValue topCardRank = cardsArray[MaxCardsInHand - 1].Value;
                CardValue nextToTheTopCardRank = cardsArray[MaxCardsInHand - 2].Value;
                CardValue bottomCardRank = cardsArray[0].Value;
                if ((nextToTheTopCardRank == CardValue.Five
                        && topCardRank == CardValue.Ace
                        && nextToTheTopCardRank - bottomCardRank == 3)
                    || (topCardRank - bottomCardRank == 4))
                {
                    isStraight = true;
                }
                if (isStraight && isFlush)
                {
                    return HandType.StraightFlush;
                }
                else if (isStraight)
                {
                    return HandType.Straight;
                }
                else if (isFlush)
                {
                    return HandType.Flush;
                }
                _handType = HandType.HighCard;
            }
            return _handType;
        }

        private void CreateCardHistogram(IEnumerable<Card> cards)
        {
            _handHistogram.Clear();
            foreach (Card card in cards)
            {
                AddToHistogram(card);
            }
        }

        private void AddToHistogram(Card card)
        {
            int cardVal = (int)card.Value;
            if (_handHistogram.ContainsKey(cardVal))
            {
                _handHistogram.Add(cardVal, _handHistogram[cardVal] + 1);
            }
            else
            {
                _handHistogram.Add(cardVal, 1);
            }
        }

        public int CompareTo(Hand otherHand)
        {
            HandType handType1 = this.GetHandType();
            HandType handType2 = otherHand.GetHandType();
            if (handType1 == HandType.Undefined || handType2 == HandType.Undefined)
            {
                // TODO: throw new Exception("");
            }
            if (handType1 == handType2)
            {
                Card[] hand1CardsArray = this.Cards.ToArray();
                Card[] hand2CardsArray = otherHand.Cards.ToArray();
                Array.Sort(hand1CardsArray);
                Array.Sort(hand2CardsArray);
                CardValue hand1TopCardRank = hand1CardsArray[MaxCardsInHand - 1].Value;
                CardValue hand2TopCardRank = hand2CardsArray[MaxCardsInHand - 1].Value;
                CardValue hand1BottomCardRank = hand1CardsArray[0].Value;
                CardValue hand2BottomCardRank = hand2CardsArray[0].Value;

                switch (handType1)
                {
                    case HandType.StraightFlush:
                    case HandType.Straight:
                        return hand1TopCardRank.CompareTo(hand2TopCardRank);
                    case HandType.Flush:
                        for (int i = MaxCardsInHand - 1; i >= 0; i--)
                        {
                            CardValue hand1CardRank = hand1CardsArray[i].Value;
                            CardValue hand2CardRank = hand2CardsArray[i].Value;
                            int flushComparisonResult = hand1CardRank.CompareTo(hand2CardRank);
                            if (flushComparisonResult != 0)
                                return flushComparisonResult;
                        }
                        return 0;
                    case HandType.FullHouse:
                        // Get the triple ranks
                        CardValue hand1TripleRank;
                        CardValue hand2TripleRank;
                        CardValue hand1PairRank;
                        CardValue hand2PairRank;
                        if (this._handHistogram[(int)hand1TopCardRank] == 3)
                        {
                            hand1TripleRank = hand1TopCardRank;
                            hand1PairRank = hand1BottomCardRank;
                        }
                        else
                        {
                            hand1TripleRank = hand1BottomCardRank;
                            hand1PairRank = hand1TopCardRank;
                        }

                        if (otherHand._handHistogram[(int)hand2TopCardRank] == 3)
                        {
                            hand2TripleRank = hand2TopCardRank;
                            hand2PairRank = hand2BottomCardRank;
                        }
                        else
                        {
                            hand2TripleRank = hand2BottomCardRank;
                            hand2PairRank = hand2TopCardRank;
                        }
                        // Compare triples. If they are equal, compare pairs
                        int fhComparisonResult = hand1TripleRank.CompareTo(hand2TripleRank);
                        if (fhComparisonResult != 0)
                        {
                            return fhComparisonResult;
                        }
                        else
                        {
                            fhComparisonResult = hand1PairRank.CompareTo(hand2PairRank);
                            return fhComparisonResult;
                        }
                    case HandType.FourOfAKind:
                        // Compare the two remaining cards and find which one of them is kicker
                        CardValue hand1Card;
                        CardValue hand2Card;
                        if (this._handHistogram[(int)hand1TopCardRank] != 4)
                        {
                            hand1Card = hand1TopCardRank;
                        }
                        else
                        {
                            hand1Card = hand1BottomCardRank;
                        }

                        if (otherHand._handHistogram[(int)hand2TopCardRank] != 4)
                        {
                            hand2Card = hand2TopCardRank;
                        }
                        else
                        {
                            hand2Card = hand2BottomCardRank;
                        }
                        return hand1Card.CompareTo(hand2Card);
                    case HandType.ThreeOfAKind:
                        // Find triples
                        CardValue? hand1ThreeOfAKindRank = null; // TODO
                        CardValue? hand2ThreeOfAKindRank = null;
                        List<CardValue> hand1RemainingCards = new List<CardValue>(2);
                        List<CardValue> hand2RemainingCards = new List<CardValue>(2);

                        for (int i = MaxCardsInHand - 1; i >= 0; i--)
                        {
                            CardValue hand1CardRank = hand1CardsArray[i].Value;
                            CardValue hand2CardRank = hand2CardsArray[i].Value;
                            if (this._handHistogram[(int)hand1CardRank] == 3)
                            {
                                hand1ThreeOfAKindRank = hand1CardRank;
                            }
                            else
                            {
                                hand1RemainingCards.Add(hand1CardRank);
                            }
                            if (otherHand._handHistogram[(int)hand2CardRank] == 3)
                            {
                                hand2ThreeOfAKindRank = hand2CardRank;
                            }
                            else
                            {
                                hand2RemainingCards.Add(hand2CardRank);
                            }
                        }
                        // FIXME: Workaround
                        int hand1ThreeOfAKindRankIntVal = hand1ThreeOfAKindRank.HasValue ? (int)hand1ThreeOfAKindRank.Value : -1;
                        int hand2ThreeOfAKindRankIntVal = hand2ThreeOfAKindRank.HasValue ? (int)hand2ThreeOfAKindRank.Value : -1;
                        int threeOfAKindComparison = hand1ThreeOfAKindRankIntVal.CompareTo(hand2ThreeOfAKindRankIntVal);
                        if (threeOfAKindComparison != 0)
                        {
                            return threeOfAKindComparison;
                        }
                        else
                        {
                            // Compare remaining cards
                            hand1RemainingCards.Sort();
                            hand2RemainingCards.Sort();
                            int remainingComparisonResult = hand1RemainingCards[1].CompareTo(hand2RemainingCards[1]);
                            if (remainingComparisonResult == 0)
                            {
                                remainingComparisonResult = hand1RemainingCards[0].CompareTo(hand2RemainingCards[0]);
                            }
                            return remainingComparisonResult;
                        }
                    case HandType.TwoPair:
                        List<CardValue> hand1PairRanks = new List<CardValue>(2);
                        List<CardValue> hand2PairRanks = new List<CardValue>(2);
                        CardValue? hand1RemainingCardRank = null;
                        CardValue? hand2RemainingCardRank = null;
                        for (int i = MaxCardsInHand - 1; i >= 0; i--)
                        {
                            CardValue hand1CardRank = hand1CardsArray[i].Value;
                            CardValue hand2CardRank = hand2CardsArray[i].Value;
                            if (this._handHistogram[(int)hand1CardRank] == 2)
                            {
                                hand1PairRanks.Add(hand1CardRank);
                            }
                            else
                            {
                                hand1RemainingCardRank = hand1CardRank;
                            }

                            if (otherHand._handHistogram[(int)hand2CardRank] == 2)
                            {
                                hand2PairRanks.Add(hand2CardRank);
                            }
                            else
                            {
                                hand2RemainingCardRank = hand2CardRank;
                            }
                        }

                        hand1PairRanks.Sort();
                        hand2PairRanks.Sort();
                        int twoPairComparisonResult = hand1PairRanks[1].CompareTo(hand2PairRanks[1]);
                        if (twoPairComparisonResult == 0)
                        {
                            twoPairComparisonResult = hand1PairRanks[0].CompareTo(hand2PairRanks[0]);
                        }

                        // FIXME: Workaround
                        int hand1RemainingCardRankIntVal = hand1RemainingCardRank.HasValue ? (int)hand1RemainingCardRank.Value : -1;
                        int hand2RemainingCardRankIntVal = hand2RemainingCardRank.HasValue ? (int)hand2RemainingCardRank.Value : -1;
                        // if the pairs are equal, determine the kicker
                        if (twoPairComparisonResult == 0)
                        {
                            twoPairComparisonResult = hand1RemainingCardRankIntVal.CompareTo(hand2RemainingCardRankIntVal);
                        }
                        return twoPairComparisonResult;
                    case HandType.OnePair:
                        CardValue? hand1OnePairRank = null;
                        CardValue? hand2OnePairRank = null;
                        List<CardValue> hand1RemainingCardRanks = new List<CardValue>(3);
                        List<CardValue> hand2RemainingCardRanks = new List<CardValue>(3);
                        for (int i = MaxCardsInHand - 1; i >= 0; i--)
                        {
                            CardValue hand1CardRank = hand1CardsArray[i].Value;
                            CardValue hand2CardRank = hand2CardsArray[i].Value;
                            if (this._handHistogram[(int)hand1CardRank] == 2)
                            {
                                hand1OnePairRank = hand1CardRank;
                            }
                            else
                            {
                                hand1RemainingCardRanks.Add(hand1CardRank);
                            }

                            if (otherHand._handHistogram[(int)hand2CardRank] == 2)
                            {
                                hand2OnePairRank = hand2CardRank;
                            }
                            else
                            {
                                hand2RemainingCardRanks.Add(hand2CardRank);
                            }
                        }
                        // if the pairs are equal, determine the kicker
                        // FIXME: Workaround
                        int onePairComparisonResult = (hand1OnePairRank.HasValue ? (int)hand1OnePairRank.Value : -1)
                            .CompareTo(hand2OnePairRank.HasValue ? (int)hand2OnePairRank.Value : -1);
                        if (onePairComparisonResult == 0)
                        {
                            hand1RemainingCardRanks.Sort();
                            hand2RemainingCardRanks.Sort();
                            onePairComparisonResult = hand1RemainingCardRanks[2].CompareTo(hand2RemainingCardRanks[2]);
                            if (onePairComparisonResult == 0)
                            {
                                onePairComparisonResult = hand1RemainingCardRanks[1].CompareTo(hand2RemainingCardRanks[1]);
                            }
                            if (onePairComparisonResult == 0)
                            {
                                onePairComparisonResult = hand1RemainingCardRanks[0].CompareTo(hand2RemainingCardRanks[0]);
                            }
                        }
                        return onePairComparisonResult;
                    case HandType.HighCard:
                        for (int i = MaxCardsInHand - 1; i >= 0; i--)
                        {
                            CardValue hand1CardRank = hand1CardsArray[i].Value;
                            CardValue hand2CardRank = hand2CardsArray[i].Value;
                            if (hand1CardRank == hand2CardRank)
                                continue;
                            if (hand1CardRank > hand2CardRank)
                                return 1;
                        }
                        break;
                }
            }
            else
            {
                return handType1.CompareTo(handType2);
            }
            return 0;
        }

        /// <summary>
        /// Evaluates the best possible hand from the array of cards
        /// </summary>
        /// <param name="cards">Array of cards to pick the best hand from</param>
        /// <returns>Hand containing only 5 cards</returns>
        public static Hand GetBestPossibleHand(List<Card> cards)
        {
            // TODO: argument check
            List<Hand> allHands = GetAllPossibleHands(cards);
            allHands.Sort();
            return allHands[allHands.Count - 1];
        }

        public static List<Hand> GetAllPossibleHands(List<Card> cards)
        {
            List<Hand> allHands = new List<Hand>();

            for (int i = 0; i < cards.Count; i++)
            {
                List<Card> newHandCards = new List<Card>(cards);
                newHandCards.RemoveAt(i);
                for (int j = 0; j < newHandCards.Count; j++)
                {
                    List<Card> newHand = new List<Card>(newHandCards);
                    newHand.RemoveAt(j);
                    allHands.Add(new Hand(newHand));
                }
            }
            return allHands;
        }
    }
}
