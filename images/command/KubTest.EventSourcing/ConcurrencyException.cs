using System;

namespace KubTest.EventSourcing
{
    /// <summary>
    /// An <see cref="Exception"/> indicating that the operation failed due to the underlying
    /// data changing, and that it is safe to retry the entire operation.
    /// </summary>
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException() : base() { }
        public ConcurrencyException(string message) : base(message) { }
        public ConcurrencyException(string message, Exception e) : base(message, e) { }
    }
}
