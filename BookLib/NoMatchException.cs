using System;
using System.Runtime.Serialization;

namespace BookLib
{
    [Serializable]
    public class NoMatchException : Exception
    {
        public NoMatchException()
        {
        }

        public NoMatchException(string message) : base(message)
        {
        }

        public NoMatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}