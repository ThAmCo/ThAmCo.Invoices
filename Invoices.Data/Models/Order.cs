using System;
using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
{
	public class Order : KeyEntity<int>
	{

		public int? InvoiceId { get; set; }

		public Invoice Invoice { get; set; }

		[Required]
		public string ProductName { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public double Price { get; set; }

		[Required]
		public DateTime PurchaseDateTime { get; set; }

	}
}