using Invoices.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.DataAccess.Repository.Invoices
{
	public interface IInvoicesRepository : IRepository<int, Invoice>
	{

		public Task<IEnumerable<Invoice>> GetAll();

	}
}