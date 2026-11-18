using System;
using System.Runtime.Serialization;

namespace BookLib
{
    [Serializable]
    public class DiscountException : Exception
    {
        public DiscountException()
        {
        }

        public DiscountException(string message) : base(message)
        {
        }

        public DiscountException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DiscountException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}