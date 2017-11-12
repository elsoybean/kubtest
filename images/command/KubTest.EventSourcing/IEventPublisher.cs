namespace KubTest.EventSourcing
{
    /// <summary>
    /// A publisher to distribute an event record to other systems.
    /// </summary>
	public interface IEventPublisher
	{
        /// <summary>
        /// Publishes the event record
        /// </summary>
        /// <param name="eventRecord">the event record</param>
		void PublishEventRecord(IEventRecord eventRecord);
	}
}