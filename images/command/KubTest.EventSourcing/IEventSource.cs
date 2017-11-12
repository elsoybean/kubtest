namespace KubTest.EventSourcing
{
    /// <summary>
    /// A model that raises events of a given type
    /// </summary>
    /// <typeparam name="T">the event type</typeparam>
	public interface IEventSource<in T> : IModel where T : IEvent
    {
        /// <summary>
        /// Apply the event to this instance
        /// </summary>
        /// <param name="evt"></param>
		void ApplyEvent(T evt);
	}
}