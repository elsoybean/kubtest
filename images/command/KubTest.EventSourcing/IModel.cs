using System;

namespace KubTest.EventSourcing
{
    /// <summary>
    /// A model
    /// </summary>
	public interface IModel
	{
        /// <summary>
        /// The model's identifier
        /// </summary>
		Guid Id { get; set; }
	}
}