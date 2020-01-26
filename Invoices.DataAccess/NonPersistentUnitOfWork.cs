using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Invoices.Data.Models;
using Invoices.DataAccess.Repository.Invoices;
using Invoices.DataAccess.Repository.Orders;
using Invoices.DataAccess.Repository.Profiles;

namespace Invoices.DataAccess
{
	public class NonPersistentUnitOfWork : IUnitOfWork
	{

		static readonly string[] productNames = new string[]
		{
				"Wrap It and Hope Cover",
				"Chocolate Cover",
				"Cloth Cover",
				"Harden Sponge Case",
				"Water Bath Case",
				"Smartphone Car Holder",
				"Sticky Tape Sport Armband",
				"Real Pencil Stylus",
				"Spray Paint Screen Protector",
				"Rippled Screen Protector",
				"Fish Scented Screen Protector",
				"Non-conductive Screen Protector"
		};

		private static readonly List<Profile> _profiles = new List<Profile>()
		{
			new Profile { Name = "Sam Hammersley - Gonsalves", Address = "1 Spaghetti Drive", Email = "sam.hammersley@outlook.com" },
			new Profile { Name = "Tom Gonsalves", Address = "18 Holey Road", Email = "t.gonsalves@ntlworld.com" },
			new Profile { Name = "Kim Hammersley - Gonsalves", Address = "5 Hello Close", Email = "k.gonsalves@ntlworld.com" },
			new Profile { Name = "Francesca Hammersley - Gonsalves", Address = "3 China Road", Email = "francescahammersley@gmail.com" }
		};

		private static readonly List<Order> _orders = new List<Order>()
		{
			new Order { InvoiceId = 1, Address = _profiles[0].Address, Name = _profiles[0].Name, Price = 0.99, ProductName = productNames[0], PurchaseDateTime = new DateTime(1995, 11, 26) },
			new Order { InvoiceId = 2, Address = _profiles[1].Address, Name = _profiles[1].Name, Price = 0.99, ProductName = productNames[1], PurchaseDateTime = new DateTime(1995, 11, 26) },
			new Order { InvoiceId = 3, Address = _profiles[2].Address, Name = _profiles[2].Name, Price = 0.99, ProductName = productNames[2], PurchaseDateTime = new DateTime(1995, 11, 26) },
			new Order { InvoiceId = 4, Address = _profiles[3].Address, Name = _profiles[3].Name, Price = 0.99, ProductName = productNames[3], PurchaseDateTime = new DateTime(1995, 11, 26) }
		};

		private static readonly List<Invoice> _invoices = new List<Invoice>()
		{
			new Invoice { State = InvoiceState.Created, Address = _profiles[0].Address, Name = _profiles[0].Name, Email = _profiles[0].Email, ProfileId = 1, InvoicedAt = new DateTime(1965, 1, 19) },
			new Invoice { State = InvoiceState.Created, Address = _profiles[1].Address, Name = _profiles[1].Name, Email = _profiles[1].Email, ProfileId = 2, InvoicedAt = new DateTime(1965, 1, 19) },
			new Invoice { State = InvoiceState.Created, Address = _profiles[2].Address, Name = _profiles[2].Name, Email = _profiles[2].Email, ProfileId = 3, InvoicedAt = new DateTime(1965, 1, 19) },
			new Invoice { State = InvoiceState.Created, Address = _profiles[3].Address, Name = _profiles[3].Name, Email = _profiles[3].Email, ProfileId = 4, InvoicedAt = new DateTime(1965, 1, 19) }
		};

		public IInvoicesRepository Invoices => new NonPersistentInvoicesRepository(_invoices);

		public IProfilesRepository Profiles => new NonPersistentProfilesRepository(_profiles);

		public IOrdersRepository Orders => new NonPersistentOrdersRepository(_orders);

		public Task Commit()
		{
			return Task.CompletedTask;
		}
	}
}