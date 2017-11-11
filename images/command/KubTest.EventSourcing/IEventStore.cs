using System;
using System.Collections.Generic;

namespace KubTest.EventSourcing
{
	public interface IEventStore
	{
		IEnumerable<EventArgs> GetAllEventsForModelId(Guid id);
		void StoreEvent(IEventRecord eventRecord);
	}
}