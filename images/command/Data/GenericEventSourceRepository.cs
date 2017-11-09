using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using command.Model;

namespace command.Data
{
	public class GenericEventSourceRepository<T> : IRepository<T> where  T : IEventSourceModel, new()
	{
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;
        private readonly ILogger _logger;

		public GenericEventSourceRepository(IEventPublisher eventPublisher, IEventStore eventStore, ILogger<GenericEventSourceRepository<T>> logger)
		{
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
			_logger = logger;
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