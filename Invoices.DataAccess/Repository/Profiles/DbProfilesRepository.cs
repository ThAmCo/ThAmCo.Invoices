using System.Linq;
using Microsoft.EntityFrameworkCore;
using Invoices.Data.Models;

namespace Invoices.DataAccess.Repository.Profiles
{
	public class DbProfilesRepository : DbSetRepository<int, Profile>, IProfilesRepository
	{

		public DbProfilesRepository(DbSet<Profile> dbSet) : base(dbSet)
		{
		}

		protected override IQueryable<Profile> Including()
		{
			return _dbSet.AsQueryable();
		}

	}
}
