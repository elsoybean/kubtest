using System;

namespace command.Model
{
	public interface IEventSource<in T> where T : EventArgs
	{
		void ApplyEvent(T eventArgs);
	}
}