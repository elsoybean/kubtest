using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using KubTest.EventSourcing;
using MongoDB.Driver;

namespace KubTest.EventStore.MongoDB
{
	public class MongoDBEventStore : IEventStore
	{
        private IMongoCollection<IEventRecord> _eventCollection;

		public MongoDBEventStore(IOptions<MongoDBOptions> optionsAccessor)
		{
            var options = optionsAccessor.Value;
            var client = new MongoClient(options.ConnectionString);
            var db = client.GetDatabase(options.Database);
            _eventCollection = db.GetCollection<IEventRecord>("events");
		}

		public IEnumerable<IEvent> GetAllEventsForModelId(Guid id)
		{
            var filter = Builders<IEventRecord>.Filter.Eq("ModelId", id);
            return _eventCollection
                .Find(filter)
                .ToList()
                .OrderBy(e => e.Event.Version)
                .Select(e => e.Event);
		}

		public void StoreEvent(IEventRecord eventRecord)
		{
            _eventCollection.InsertOne(eventRecord);
		}
	}
}