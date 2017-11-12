using System;
using System.Collections.Generic;

namespace KubTest.EventSourcing
{
    /// <summary>
    /// A store for events
    /// </summary>
	public interface IEventStore
	{
        /// <summary>
        /// Get all events for a given model
        /// </summary>
        /// <param name="id">the model's identifier</param>
        /// <returns>all events for the given model</returns>
		IEnumerable<IEvent> GetAllEventsForModelId(Guid id);

        /// <summary>
        /// Stores an event record
        /// </summary>
        /// <param name="eventRecord">the event record</param>
		void StoreEvent(IEventRecord eventRecord);
	}
}