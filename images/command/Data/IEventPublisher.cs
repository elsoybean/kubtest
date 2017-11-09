namespace command.Data
{
	public interface IEventPublisher
	{
		void PublishEventRecord(IEventRecord eventRecord);
	}
}