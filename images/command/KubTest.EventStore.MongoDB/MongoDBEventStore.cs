using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Options;
using KubTest.EventSourcing;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using KubTest.Model;

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

            BsonClassMap.RegisterClassMap<EventRecord>(cm => {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });

            //BsonClassMap.RegisterClassMap<FooCreated>();
            Assembly.Load(new AssemblyName("KubTest.Model"))
                .DefinedTypes
                .Where(ti => ti.ImplementedInterfaces.Contains(typeof(IEvent)))
                .Select(ti => ti.AsType())
                .ToList()
                .ForEach(t => {
                    var cm = new BsonClassMap(t);
                    cm.AutoMap();
                    BsonClassMap.RegisterClassMap(cm);
                });
        }

		public IEnumerable<IEvent> GetAllEventsForModelId(Guid id)
		{
            var filter = Builders<IEventRecord>.Filter.Eq("ModelId", id);
            return _eventCollection
                .Find(filter)
                .ToList()
                .OrderBy(e => e.Event.__Serial)
                .Select(e => e.Event);
		}

		public void StoreEvent(IEventRecord eventRecord)
		{
            _eventCollection.InsertOne(eventRecord);
		}
	}
}