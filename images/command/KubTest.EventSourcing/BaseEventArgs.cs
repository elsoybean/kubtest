using System;

namespace KubTest.EventSourcing
{
    public class BaseEvent : IEvent
    {
        public DateTime EventOccurred { get; private set; }
        public int Version { get; internal set; }

		public BaseEvent()
		{
			EventOccurred = DateTime.Now;
		}
	}
}