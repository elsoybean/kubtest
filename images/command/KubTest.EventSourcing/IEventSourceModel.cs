using System;
using System.Collections.Generic;

namespace KubTest.EventSourcing
{
	public interface IEventSourceModel : IModel
	{
		void Raise<T>(T evt) where T : EventArgs;
		void ApplyAllEvents(IEnumerable<EventArgs> events);
		IEnumerable<EventArgs> Commit();
	}
}