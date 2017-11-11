using System;

namespace KubTest.EventSourcing
{
	public class BaseEventArgs : EventArgs
	{
		public DateTime EventOccurred { get; private set; }

		public BaseEventArgs()
		{
			EventOccurred = DateTime.Now;
		}
	}
}