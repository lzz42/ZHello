using System;
using System.Runtime.Serialization;

namespace WService
{
    [Serializable]
    internal class QuoteException : Exception
    {
        public QuoteException()
        {

        }

        public QuoteException(string message) : base(message)
        {

        }

        public QuoteException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected QuoteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}