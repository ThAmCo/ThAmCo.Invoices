using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.Data.Persistence
{
	public class InvoicesDbInitialiser
	{

		public static async Task SeedTestData(InvoicesDbContext context, IServiceProvider services)
		{

			var productNames = new string[]
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

			var invoices = new List<Invoice>();
			var orders = new Order[4];

			for (int i = 0; i < 4; i++)
			{ 
				invoices.Add(new Invoice { UserId = "492d9bb2-a82f-4752-abbf-561872142bd1", State = InvoiceState.Created, Address = "18 Lois Lane", Name = "Samuel Spaghetti Hammersley", Email = "sam.hammersley@outlook.com", InvoicedAt = new DateTime(1965, 1, 19) });

				orders[i] = new Order { InvoiceId = i + 1, Address = "18 Lois Lane", Name = "Samuel Spaghetti Hammersley", Price = 0.99, ProductName = productNames[i + 1], PurchaseDateTime = new DateTime(1995, 11, 26) };
			}

			if (!context.Invoices.Any())
			{
				invoices.ForEach(p => context.Invoices.Add(p));
				await context.SaveChangesAsync();
			}

			if (!context.Orders.Any())
			{
				for (int i = 0; i < orders.Length; i++)
				{
					context.Orders.Add(orders[i]);
				}
				await context.SaveChangesAsync();
			}
		}

	}
}
