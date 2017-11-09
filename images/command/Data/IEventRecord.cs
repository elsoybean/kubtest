using System;

namespace command.Data
{
	public interface IEventRecord
	{		
		Guid ModelId { get; }
		string EventType { get; }
		EventArgs EventArgs { get; }
	}
}