using System.Threading.Tasks;
using Invoices.Data.Persistence;
using Invoices.DataAccess.Repository.Invoices;
using Invoices.DataAccess.Repository.Orders;

namespace Invoices.DataAccess
{
	public class InvoicesDbUnitOfWork : IUnitOfWork
	{

		private readonly InvoicesDbContext _context;

		private DbOrdersRepository _ordersRepository;

		private DbInvoicesRepository _invoiceRepository;

		public InvoicesDbUnitOfWork(InvoicesDbContext context)
		{
			_context = context;
		}

		public IInvoicesRepository Invoices
		{
			get
			{
				if (_invoiceRepository == null)
				{
					_invoiceRepository = new DbInvoicesRepository(_context.Invoices);
				}
				return _invoiceRepository;
			}
		}

		public IOrdersRepository Orders
		{
			get
			{
				if (_ordersRepository == null)
				{
					_ordersRepository = new DbOrdersRepository(_context.Orders);
				}
				return _ordersRepository;
			}
		}

		public async Task Commit()
		{
			await _context.SaveChangesAsync();
		}
	}
}