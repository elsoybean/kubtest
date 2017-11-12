using System;
using System.Linq;

namespace KubTest.EventSourcing
{
    /// <summary>
    /// An implementation of <see cref="IRepository{T}"/> to retrieve and store an event source based  model
    /// </summary>
    /// <typeparam name="T">the model type</typeparam>
	public class EventSourceRepository<T> : IRepository<T> where  T : BaseEventSourceModel, new()
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventStore _eventStore;

        /// <summary>
        /// Constructor for <see cref="EventSourceRepository"/>
        /// </summary>
        /// <param name="eventPublisher">the event publisher</param>
        /// <param name="eventStore">the event store</param>
		public EventSourceRepository(IEventPublisher eventPublisher, IEventStore eventStore)
		{
            _eventPublisher = eventPublisher;
            _eventStore = eventStore;
		}

        /// <summary>
        /// Gets a model by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>the model</returns>
		public T GetById(Guid id)
		{
            var eventList = _eventStore.GetAllEventsForModelId(id);

            if (!eventList.Any())
                return null;

            var model = new T
            {
                Id = id,
                __serial = eventList.Select(e => e.__Serial).DefaultIfEmpty().Max(),
            };

            model.ApplyAllEvents(eventList);

			return model;
		}

        /// <summary>
        /// Saves the model
        /// </summary>
        /// <param name="model">the model</param>
		public void Save(T model)
		{
			var events = model.Commit();
            if (model.__serial == 0 && !events.Any())
                throw new InvalidOperationException("Cannot save new model with no events");

            var eventList = _eventStore.GetAllEventsForModelId(model.Id);
            if (eventList.Count() != model.__serial)
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