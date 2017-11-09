using System;
using command.Model;

namespace command.Data
{
	public class EventRecord : IEventRecord
	{		
		public Guid ModelId { get; private set; }
		public string EventType { get; private set; }
		public EventArgs EventArgs { get; private set; }

		private EventRecord(Guid modelId, string eventType, EventArgs eventArgs)
		{
			ModelId = modelId;
			EventType = eventType;
			EventArgs = eventArgs;
		}

		public static IEventRecord Create<TModel>(TModel model, EventArgs eventArgs) where TModel : IModel
		{
			var eventType = string.Format("{0}.{1}", typeof(TModel).Name, eventArgs.GetType().Name);
			return new EventRecord(model.Id, eventType, eventArgs);
		}
	}
}