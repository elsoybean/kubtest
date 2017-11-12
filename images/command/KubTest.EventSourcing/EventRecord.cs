using System;

namespace KubTest.EventSourcing
{
    /// <summary>
    /// A wrapper for an event to carry additional metadata associated with it.
    /// </summary>
	public class EventRecord : IEventRecord
	{
        public Guid ModelId { get; private set; }
        public string EventType { get; private set; }
        public IEvent Event { get; private set; }

        /// <summary>
        /// Constructor for <see cref="EventRecord"/>.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="eventType"></param>
        /// <param name="evt"></param>
		private EventRecord(Guid modelId, string eventType, IEvent evt)
		{
			ModelId = modelId;
			EventType = eventType;
			Event = evt;
		}

        /// <summary>
        /// Factory method to create event records
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <param name="evt"></param>
        /// <returns></returns>
		public static EventRecord Create<TModel>(TModel model, IEvent evt) where TModel : IModel
		{
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id == Guid.Empty)
                throw new ArgumentNullException("model.Id");

            if (evt == null)
                throw new ArgumentNullException(nameof(evt));

            var eventType = string.Format("{0}.{1}", typeof(TModel).Name, evt.GetType().Name);
			return new EventRecord(model.Id, eventType, evt);
		}
	}
}