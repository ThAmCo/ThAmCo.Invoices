namespace Invoices.App.Models
{
	public class CreateInvoiceModel
	{

		public string UserId { get; set; }

		public string Address { get; set; }

		public string Email { get; set; }

		public string Name { get; set; }

		public static CreateInvoiceModel FromPostOrderRequest(PostOrderRequest request)
		{
			return new CreateInvoiceModel
			{
				UserId = request.UserId,
				Address = request.Address,
				Email = request.Email,
				Name = request.Name
			};
		}

	}
}