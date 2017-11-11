using System;
using System.Linq;
using System.Collections.Generic;

namespace KubTest.EventSourcing
{
	public class GenericEventSourceRepository<T> : IRepository<T> where  T : IEventSourceModel, new()
	{
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;

		public GenericEventSourceRepository(IEventPublisher eventPublisher, IEventStore eventStore)
		{
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
		}

		public T GetById(Guid id)
		{
			var eventList = _eventStore.GetAllEventsForModelId(id);
			var model = new T();
			model.Id = id;
			model.ApplyAllEvents(eventList);

			return model;
		}

		public void Save(T model)
		{
			var events = model.Commit();
			events.Select(e => EventRecord.Create(model, e))
				.ToList()
				.ForEach(r =>
					{
						_eventStore.StoreEvent(r);
						_eventPublisher.PublishEventRecord(r);
					});
		}
	}
}