using System;

namespace KubTest.EventSourcing
{
	public interface IModel
	{
		Guid Id { get; set; }
	}
}