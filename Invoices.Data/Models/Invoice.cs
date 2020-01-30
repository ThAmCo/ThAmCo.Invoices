using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Invoices.Data.Models
{
	public class Invoice : KeyEntity<int>
	{

		[Required]
		public InvoiceState State { get; set; }

		[Required]
		public string UserId { get; set; }

		[Required]
		public string Name { get; set; }

		[Required, DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		public string Address { get; set; }

		[Required]
		public DateTime InvoicedAt { get; set; }

		public virtual List<Order> Orders { get; set; }

		public double TotalPrice => Orders.Select(o => o.Price).Sum();

	}
}