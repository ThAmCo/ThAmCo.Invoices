﻿using Microsoft.EntityFrameworkCore;
using Invoices.Data.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.DataAccess.Repository
{
	public abstract class DbSetRepository<K, T> : IRepository<K, T> where T : KeyEntity<K>
	{
		protected readonly DbSet<T> _dbSet;

		public DbSetRepository(DbSet<T> dbSet)
		{
			_dbSet = dbSet;
		}

		protected abstract IQueryable<T> Including();

		public async Task<T> Get(K key)
		{
			return await Including().Where(t => t.Id.Equals(key)).FirstAsync();
		}

		public void Create(T t)
		{
			_dbSet.Add(t);
		}

		public void Remove(T t)
		{
			_dbSet.Remove(t);
		}

		public void Update(T t)
		{
			_dbSet.Update(t);
		}

	}
}
