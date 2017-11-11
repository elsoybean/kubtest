using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace KubTest.EventSourcing
{
	public class AbstractEventSourceModel : IEventSourceModel
	{
		public Guid Id { get; set; }

		private List<EventArgs> _uncommittedEvents = new List<EventArgs>();

		public void Raise<T>(T evt) where T : EventArgs
		{
			ApplyGenericEvent(evt);
			_uncommittedEvents.Add(evt);
		}

		public void ApplyAllEvents(IEnumerable<EventArgs> events)
		{
			events.ToList().ForEach(ApplyGenericEvent);
		}

		public IEnumerable<EventArgs> Commit()
		{
			var commitingEvents = new List<EventArgs>(_uncommittedEvents);
			_uncommittedEvents.Clear();
			return commitingEvents;
		}

		protected void ApplyGenericEvent(EventArgs evt)
		{
			try
			{
				typeof(IEventSource<>).MakeGenericType(evt.GetType()).GetMethod("ApplyEvent").Invoke(this, new[] { evt });
			}
			catch (Exception e)
			{
				throw new InvalidOperationException(string.Format("model type {0} cannot handle event type {1}", this.GetType().Name, evt.GetType().Name), e);
			}
		}
	}
}