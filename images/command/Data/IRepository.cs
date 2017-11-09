using System;
using command.Model;

namespace command.Data
{
	public interface IRepository<T> where  T : IModel
	{
		T GetById(Guid id);
		void Save(T model);
	}
}