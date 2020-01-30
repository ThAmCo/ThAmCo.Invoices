using Invoices.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.DataAccess.Repository.Invoices
{
	public class DbInvoicesRepository : DbSetRepository<int, Invoice>, IInvoicesRepository
	{
		public DbInvoicesRepository(DbSet<Invoice> dbSet) : base(dbSet)
		{

		}

		protected override IQueryable<Invoice> Including()
		{
			return _dbSet.Include(i => i.Orders);
		}

		public async Task<IEnumerable<Invoice>> GetAll()
		{
			return await Including().ToListAsync();
		}

		public async Task<IEnumerable<Invoice>> ForUser(string userId)
		{
			return await Including().Where(i => i.UserId.ToLower().Equals(userId.ToLower())).ToListAsync();
		}
	}
}