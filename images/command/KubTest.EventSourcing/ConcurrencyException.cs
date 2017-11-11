using System;

namespace KubTest.EventSourcing
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException() : base() { }
        public ConcurrencyException(string message) : base(message) { }
        public ConcurrencyException(string message, Exception e) : base(message, e) { }
    }
}
