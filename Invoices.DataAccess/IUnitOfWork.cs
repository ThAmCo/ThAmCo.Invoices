using Invoices.DataAccess.Repository.Invoices;
using Invoices.DataAccess.Repository.Orders;
using System.Threading.Tasks;

namespace Invoices.DataAccess
{
	public interface IUnitOfWork
	{
		public IInvoicesRepository Invoices { get; }

		public IOrdersRepository Orders { get; }

		public Task Commit();

	}
}