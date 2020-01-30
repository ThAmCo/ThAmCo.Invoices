using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Invoices.Data.Models;

namespace Invoices.DataAccess.Repository.Invoices
{
	public class NonPersistentInvoicesRepository : NonPersistentRepository<Invoice>, IInvoicesRepository
	{
		public NonPersistentInvoicesRepository(List<Invoice> elements) : base(elements)
		{
		}

		public Task<IEnumerable<Invoice>> GetAll()
		{
			return Task.FromResult(_elements.AsEnumerable());
		}

		public Task<IEnumerable<Invoice>> ForUser(string userId)
		{
			return Task.FromResult(_elements.Where(i => i.UserId.Equals(userId)));
		}

	}
}
