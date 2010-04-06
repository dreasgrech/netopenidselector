using System;
using System.Runtime.Serialization;

namespace OpenIDSelector
{
    [Serializable]
    public class InvalidProviderURLException : Exception
    {
        public InvalidProviderURLException()
        {
        }

        public InvalidProviderURLException(string message)
            : base(message)
        {
        }

        public InvalidProviderURLException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected InvalidProviderURLException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
