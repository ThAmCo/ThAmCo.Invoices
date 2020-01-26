namespace Invoices.Data.Models
{
	public enum InvoiceState
	{
		/// <summary>
		/// The invoice exists and has been sent to the customer.
		/// </summary>
		Sent,

		/// <summary>
		/// The invoice exists but has not yet been sent.
		/// </summary>
		Created,

		/// <summary>
		/// The invoice has not yet been finalised.
		/// </summary>
		Unfinished

	}
}