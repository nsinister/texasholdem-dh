using System;
using System.Collections.Generic;
using System.Text;

namespace Darkhood.TexasHoldem.Core
{
    public class HandFullException : Exception
    {
        public HandFullException() : base()
        {
        }

        public HandFullException(string message) : base(message)
        {
        }
    }
}
