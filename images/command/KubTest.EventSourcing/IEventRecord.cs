using System;

namespace KubTest.EventSourcing
{
    /// <summary>
    /// A wrapper for an <see cref="IEvent"/> to carry additional metadata associated with the event.
    /// </summary>
	public interface IEventRecord
	{
        /// <summary>
        /// The Identifier of the model that raised the event
        /// </summary>
		Guid ModelId { get; }

        /// <summary>
        /// The type of event
        /// </summary>
		string EventType { get; }

        /// <summary>
        /// The event
        /// </summary>
        IEvent Event { get; }
	}
}