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

			var profiles = new Profile[]
			{
				new Profile { Name = "Sam Hammersley - Gonsalves", Address = "1 Spaghetti Drive", Email = "sam.hammersley@outlook.com" },
				new Profile { Name = "Tom Gonsalves", Address = "18 Holey Road", Email = "t.gonsalves@ntlworld.com" },
				new Profile { Name = "Kim Hammersley - Gonsalves", Address = "5 Hello Close", Email = "k.gonsalves@ntlworld.com" },
				new Profile { Name = "Francesca Hammersley - Gonsalves", Address = "3 China Road", Email = "francescahammersley@gmail.com" }
			};

			var invoices = new List<Invoice>();
			var orders = new Order[4];

			for (int i = 0; i < profiles.Length; i++)
			{ 
				var prof = profiles[i];
				invoices.Add(new Invoice { State = InvoiceState.Created, Address = prof.Address, Name = prof.Name, Email = prof.Email, ProfileId = i + 1, InvoicedAt = new DateTime(1965, 1, 19) });

				orders[i] = new Order { InvoiceId = i + 1, Address = prof.Address, Name = prof.Name, Price = 0.99, ProductName = productNames[i + 1], PurchaseDateTime = new DateTime(1995, 11, 26) };
			}

			if (!context.Profiles.Any())
			{
				for (int i = 0; i < profiles.Length; i++)
				{
					context.Profiles.Add(profiles[i]);
				}
				await context.SaveChangesAsync();
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
