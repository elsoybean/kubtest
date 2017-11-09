using System;
using System.Collections.Generic;

namespace command.Data
{
	public interface IEventStore
	{
		IEnumerable<EventArgs> GetAllEventsForModelId(Guid id);
		void StoreEvent(IEventRecord eventRecord);
	}
}