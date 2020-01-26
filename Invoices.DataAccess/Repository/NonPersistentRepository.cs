using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.DataAccess.Repository
{
	public class NonPersistentRepository<T> : IRepository<int, T> where T : class
	{

		protected readonly List<T> _elements;

		public NonPersistentRepository(List<T> elements)
		{
			_elements = elements;
		}

		public Task<T> Get(int key)
		{
			T value = _elements[key];

			return Task.FromResult(value);
		}

		public void Create(T t)
		{
			_elements.Add(t);
		}

		public void Remove(T t)
		{
			_elements.Remove(t);
		}

		public void Update(T t)
		{
			int index = _elements.IndexOf(t);

			if (index < 0)
			{
				_elements.Add(t);
			} else
			{
				_elements.RemoveAt(index);
				_elements.Insert(index, t);
			}
		}
	}
}