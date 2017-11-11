using System;

namespace KubTest.EventSourcing
{
	public interface IEventRecord
	{		
		Guid ModelId { get; }
		string EventType { get; }
		EventArgs EventArgs { get; }
	}
}