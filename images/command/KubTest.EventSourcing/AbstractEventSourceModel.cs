using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace KubTest.EventSourcing
{
	public class AbstractEventSourceModel : IEventSourceModel
	{
		public Guid Id { get; set; }

		private List<IEvent> _uncommittedEvents = new List<IEvent>();

        public int Version { get; internal set; }

        private readonly object _lock = new object();
        private bool _isCommitted = false;

		public void Raise<T>(T evt) where T : IEvent
        {
            if (_isCommitted)
                throw new InvalidOperationException("the model has already been committed");

            lock (_lock)
            {
                var baseEvent = evt as BaseEvent;
                if (baseEvent != null)
                    baseEvent.Version = Version + _uncommittedEvents.Count + 1;

                ApplyGenericEvent(evt);
			    _uncommittedEvents.Add(evt);
            }
        }

        public void ApplyAllEvents(IEnumerable<IEvent> events)
		{
			events.ToList().ForEach(ApplyGenericEvent);
		}

		public IEnumerable<IEvent> Commit()
		{
            lock (_lock)
            {
                var commitingEvents = new List<IEvent>(_uncommittedEvents);
                _uncommittedEvents.Clear();
			    return commitingEvents;
            }
        }

        protected void ApplyGenericEvent(IEvent evt)
		{
			try
			{
                typeof(IEventSource<>).MakeGenericType(evt.GetType()).GetMethod("ApplyEvent").Invoke(this, new[] { evt });
			}
			catch (TargetException e)
			{
				throw new InvalidOperationException(string.Format("model type {0} cannot handle event type {1}", this.GetType().Name, evt.GetType().Name), e);
			}
		}
	}
}