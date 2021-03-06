using Invoices.Data.Models;

using System;

namespace Invoices.App.Models
{
	public class PostOrderRequest
	{

		public string UserId { get; set; }

		public string ProductName { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }

		public double Price { get; set; }

		public string Email { get; set; }

		public DateTime PurchaseDateTime { get; set; }

	}
}