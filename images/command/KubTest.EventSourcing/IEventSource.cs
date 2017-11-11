namespace KubTest.EventSourcing
{
	public interface IEventSource<in T> where T : IEvent
    {
		void ApplyEvent(T eventArgs);
	}
}