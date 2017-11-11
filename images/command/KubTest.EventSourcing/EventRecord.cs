using System;

namespace KubTest.EventSourcing
{
	public class EventRecord : IEventRecord
	{
        public Guid Id { get; set; }
		public Guid ModelId { get; private set; }
		public string EventType { get; private set; }
		public IEvent Event { get; private set; }

		private EventRecord(Guid modelId, string eventType, IEvent evt)
		{
            Id = Guid.NewGuid();
			ModelId = modelId;
			EventType = eventType;
			Event = evt;
		}

		public static IEventRecord Create<TModel>(TModel model, IEvent evt) where TModel : IModel
		{
			var eventType = string.Format("{0}.{1}", typeof(TModel).Name, evt.GetType().Name);
			return new EventRecord(model.Id, eventType, evt);
		}
	}
}