using System;
using System.Collections.Generic;

namespace KubTest.EventSourcing
{
	public interface IEventStore
	{
		IEnumerable<IEvent> GetAllEventsForModelId(Guid id);
		void StoreEvent(IEventRecord eventRecord);
	}
}