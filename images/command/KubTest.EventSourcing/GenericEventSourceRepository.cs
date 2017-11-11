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

            var baseModel = model as AbstractEventSourceModel;
            if (baseModel != null)
                baseModel.Version = eventList.Count();

			model.ApplyAllEvents(eventList);

			return model;
		}

		public void Save(T model)
		{
			var events = model.Commit();
            if (model.Version == 0 && !events.Any())
                throw new InvalidOperationException("Cannot save new model with no events");

            var eventList = _eventStore.GetAllEventsForModelId(model.Id);
            if (eventList.Count() != model.Version)
                throw new ConcurrencyException("Model is not at the latest version; reload the model and retry the operation");

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