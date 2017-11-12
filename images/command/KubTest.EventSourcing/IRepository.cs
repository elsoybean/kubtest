using System;

namespace KubTest.EventSourcing
{
    /// <summary>
    /// A repository to store and retrieve instances of <see cref="IModel"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IRepository<T> where  T : IModel
	{
        /// <summary>
        /// Gets an instance of <see cref="IModel"/> by identifier
        /// </summary>
        /// <param name="id">the identifier</param>
        /// <returns>the model</returns>
		T GetById(Guid id);

        /// <summary>
        /// Saves the <see cref="IModel"/>
        /// </summary>
        /// <param name="model">the model</param>
		void Save(T model);
	}
}