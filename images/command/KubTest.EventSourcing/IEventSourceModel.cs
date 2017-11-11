using System.Collections.Generic;

namespace KubTest.EventSourcing
{
	public interface IEventSourceModel : IModel
	{
        int Version { get; }
		void Raise<T>(T evt) where T : IEvent;
		void ApplyAllEvents(IEnumerable<IEvent> events);
		IEnumerable<IEvent> Commit();
	}
}