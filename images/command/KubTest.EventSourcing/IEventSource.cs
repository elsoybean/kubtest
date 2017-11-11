using System;

namespace KubTest.EventSourcing
{
	public interface IEventSource<in T> where T : EventArgs
	{
		void ApplyEvent(T eventArgs);
	}
}