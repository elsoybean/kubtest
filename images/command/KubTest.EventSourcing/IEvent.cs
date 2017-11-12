using System;

namespace KubTest.EventSourcing
{
    /// <summary>
    /// An event raised by a model
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// The date and time at which the event occurred
        /// </summary>
        DateTime EventOccurred { get; }

        /// <summary>
        /// A serial value indicating the order in which this event was raised by a model
        /// </summary>
        int __Serial { get; }
    }
}
