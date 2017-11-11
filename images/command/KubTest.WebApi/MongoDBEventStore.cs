using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using KubTest.EventSourcing;
using KubTest.Model;

namespace KubTest.WebApi
{
	public class MongoDBEventStore : IEventStore
	{
        private readonly ILogger _logger;

		public MongoDBEventStore(ILogger<MongoDBEventStore> logger)
		{
			_logger = logger;
		}

		public IEnumerable<EventArgs> GetAllEventsForModelId(Guid id)
		{
			return new List<EventArgs>()
			{
				new FooCreated("blue"),
				new ColorChanged(null, "red"),
			};
		}

		public void StoreEvent(IEventRecord eventRecord)
		{
			_logger.LogDebug(string.Format("{0} on {1} saved", eventRecord.EventType, eventRecord.ModelId));
		}
	}
}