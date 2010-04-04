using System;
using System.Runtime.Serialization;

namespace OpenIDSelector.Exceptions
{
    [Serializable]
    public class InvalidURLException : Exception
    {
        public InvalidURLException()
        {
        }

        public InvalidURLException(string message)
            : base(message)
        {
        }

        public InvalidURLException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidURLException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
