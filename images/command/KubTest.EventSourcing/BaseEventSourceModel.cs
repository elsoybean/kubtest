using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace KubTest.EventSourcing
{
    /// <summary>
    /// A base class for <see cref="IModel"/> implementing the infrastructure for the event sourcing framework.
    /// </summary>
	public class BaseEventSourceModel : IModel
	{
        /// <summary>
        /// The model's Identifier
        /// </summary>
		public Guid Id { get; set; }

        /// <summary>
        /// A serial value indicating the model's version
        /// </summary>
        internal int __serial { get; set; }

        private List<IEvent> _uncommittedEvents = new List<IEvent>();
        private bool _isCommitted = false;

        /// <summary>
        /// Raises an event
        /// </summary>
        /// <typeparam name="T">the type of event</typeparam>
        /// <param name="evt">the event</param>
		protected void Raise<T>(T evt) where T : BaseEvent
        {
            if (_isCommitted)
                throw new InvalidOperationException("the model has already been committed");

            evt.__Serial = __serial + _uncommittedEvents.Count + 1;
            ApplyGenericEvent(evt);
			_uncommittedEvents.Add(evt);
        }

        /// <summary>
        /// Applies all of the given existing events to the model
        /// </summary>
        /// <param name="events"></param>
        public void ApplyAllEvents(IEnumerable<IEvent> events)
		{
			events.ToList().ForEach(ApplyGenericEvent);
		}

        /// <summary>
        /// Commits the events that have been raised by this instance
        /// </summary>
        /// <returns>the list of events that have been raised by this instance</returns>
		internal IEnumerable<IEvent> Commit()
		{
            var commitingEvents = new List<IEvent>(_uncommittedEvents);
            _uncommittedEvents.Clear();
			return commitingEvents;
        }

        private void ApplyGenericEvent(IEvent evt)
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