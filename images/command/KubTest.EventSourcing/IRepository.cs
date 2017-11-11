using System;

namespace KubTest.EventSourcing
{
	public interface IRepository<T> where  T : IModel
	{
		T GetById(Guid id);
		void Save(T model);
	}
}