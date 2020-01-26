using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Invoices.Data.Models;
using System;

namespace Invoices.DataAccess.Repository.Orders
{
	public class DbOrdersRepository : DbSetRepository<int, Order>, IOrdersRepository
	{

		public DbOrdersRepository(DbSet<Order> dbSet) : base(dbSet)
		{
		}

		protected override IQueryable<Order> Including()
		{
			return _dbSet.AsQueryable();
		}

		public async Task<IEnumerable<Order>> GetAll()
		{
			return await Including().ToListAsync();
		}

		public async Task<IEnumerable<Order>> GetOrdersAfter(DateTime dateTime)
		{
			return await Including()
				.Where(o => o.PurchaseDateTime.CompareTo(dateTime) > 0)
				.OrderByDescending(o => o.PurchaseDateTime)
				.ToListAsync();
		}
	}
}
