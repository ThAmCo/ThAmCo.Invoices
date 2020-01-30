using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Invoices.Data.Models;
using Invoices.DataAccess.Repository.Invoices;
using Invoices.DataAccess.Repository.Orders;

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

		private static readonly List<Order> _orders = new List<Order>()
		{
			new Order { InvoiceId = 1, Address = "18 Lois Lane", Name = "Samuel Spaghetti Hammersley", Price = 0.99, PurchaseDateTime = new DateTime(1995, 11, 26), ProductName = productNames[0] },
			new Order { InvoiceId = 2, Address = "18 Lois Lane", Name = "Samuel Spaghetti Hammersley", Price = 0.99, PurchaseDateTime = new DateTime(1995, 11, 26), ProductName = productNames[1] },
			new Order { InvoiceId = 3, Address = "18 Lois Lane", Name = "Samuel Spaghetti Hammersley", Price = 0.99, PurchaseDateTime = new DateTime(1995, 11, 26), ProductName = productNames[2] },
			new Order { InvoiceId = 4, Address = "18 Lois Lane", Name = "Samuel Spaghetti Hammersley", Price = 0.99, PurchaseDateTime = new DateTime(1995, 11, 26), ProductName = productNames[3] }
		};

		private static readonly List<Invoice> _invoices = new List<Invoice>()
		{
			new Invoice { State = InvoiceState.Created, Address = "18 Lois Lane", Name = "Samuel Spaghetti Hammersley", Email = "sam.hammersley@outlook.com", InvoicedAt = new DateTime(1965, 1, 19) },
			new Invoice { State = InvoiceState.Created, Address = "18 Lois Lane", Name = "Samuel Spaghetti Hammersley", Email = "sam.hammersley@outlook.com", InvoicedAt = new DateTime(1965, 1, 19) },
			new Invoice { State = InvoiceState.Created, Address = "18 Lois Lane", Name = "Samuel Spaghetti Hammersley", Email = "sam.hammersley@outlook.com", InvoicedAt = new DateTime(1965, 1, 19) },
			new Invoice { State = InvoiceState.Created, Address = "18 Lois Lane", Name = "Samuel Spaghetti Hammersley", Email = "sam.hammersley@outlook.com", InvoicedAt = new DateTime(1965, 1, 19) }
		};

		public IInvoicesRepository Invoices => new NonPersistentInvoicesRepository(_invoices);

		public IOrdersRepository Orders => new NonPersistentOrdersRepository(_orders);

		public Task Commit()
		{
			return Task.CompletedTask;
		}
	}
}