using System;
using System.Runtime.Serialization;

namespace BookLib
{
    [Serializable]
    public class MissingItemException : Exception
    {
        public MissingItemException()
        {
        }

        public MissingItemException(string message) : base(message)
        {
        }

        public MissingItemException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingItemException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}