using Invoices.Data.Models;
using System.Collections.Generic;

namespace Invoices.App.Models
{
	public class InvoicesIndexViewModel
	{

		public IEnumerable<Invoice> Invoices { get; set; }

		public InvoicesIndexRedirectState RedirectState { get; set; }

	}
}