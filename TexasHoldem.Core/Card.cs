using System;
using System.Collections.Generic;
using System.Text;

namespace TexasHoldem.Core
{
    public class Card : IComparable<Card>, IEquatable<Card>
    {
        public CardSuit Suit { get; private set; }
        public CardValue Value { get; private set; }

        public Card(CardSuit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }

        public override string ToString()
        {
            // TODO: Localization
            string valueStr = Enum.GetName(typeof(CardValue), Value);
            string suitStr = Enum.GetName(typeof(CardSuit), Suit);
            return String.Format("{0} {1}", valueStr, suitStr);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash *= 17 + (int)Suit;
            hash *= 31 + (int)Value;
            return hash;
        }

        public bool Equals(Card other)
        {
            if (Suit != other.Suit)
            {
                return false;
            }
            if (Value != other.Value)
            {
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null || obj.GetType() != typeof(Card))
            {
                return false;
            }
            Card otherCard = (Card)obj;
            return Equals(otherCard);
        }

        public int CompareTo(Card other)
        {
            throw new NotImplementedException();
        }
    }
}
