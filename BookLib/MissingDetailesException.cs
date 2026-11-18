using System;
using System.Runtime.Serialization;

namespace BookLib
{
    [Serializable]
    public class MissingDetailesException : Exception
    {
        public MissingDetailesException()
        {
        }

        public MissingDetailesException(string message) : base(message)
        {
        }

        public MissingDetailesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MissingDetailesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}