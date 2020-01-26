using System.Threading.Tasks;
using Invoices.Data.Persistence;
using Invoices.DataAccess.Repository.Invoices;
using Invoices.DataAccess.Repository.Profiles;
using Invoices.DataAccess.Repository.Orders;

namespace Invoices.DataAccess
{
	public class InvoicesDbUnitOfWork : IUnitOfWork
	{

		private readonly InvoicesDbContext _context;

		private IOrdersRepository _ordersRepository;

		private IProfilesRepository _profilesRepository;

		private IInvoicesRepository _invoiceRepository;

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

		public IProfilesRepository Profiles
		{
			get
			{
				if (_profilesRepository == null)
				{
					_profilesRepository = new DbProfilesRepository(_context.Profiles);
				}
				return _profilesRepository;
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