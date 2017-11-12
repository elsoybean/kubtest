using System;

namespace KubTest.EventSourcing
{
    /// <summary>
    /// A base class for <see cref="IEvent"/> implementing the infrastructure for the event sourcing framework.
    /// </summary>
    public class BaseEvent : IEvent
    {
        /// <summary>
        /// The date and time at which the event occurred
        /// </summary>
        public DateTime EventOccurred { get; private set; }

        /// <summary>
        /// A serial value indicating the order in which this event was raised by a model
        /// </summary>
        public int __Serial { get; internal set; }

        /// <summary>
        /// Constructor for <see cref="BaseEvent"/>
        /// </summary>
		public BaseEvent()
		{
			EventOccurred = DateTime.Now;
		}
	}
}