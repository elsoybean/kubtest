namespace KubTest.EventSourcing
{
	public interface IEventPublisher
	{
		void PublishEventRecord(IEventRecord eventRecord);
	}
}