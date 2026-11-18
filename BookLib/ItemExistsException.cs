using System;
using System.Runtime.Serialization;

namespace BookLib
{
    [Serializable]
    public class ItemExistsException : Exception
    {
        public ItemExistsException()
        {
        }

        public ItemExistsException(string message) : base(message)
        {
        }

        public ItemExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ItemExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}