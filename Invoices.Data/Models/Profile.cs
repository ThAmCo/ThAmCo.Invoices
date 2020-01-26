using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models
{
	public class Profile : KeyEntity<int>
	{
		[Required]
		public string Name { get; set; }

		[DataType(DataType.EmailAddress), Required]
		public string Email { get; set; }

		[Required]
		public string Address { get; set; }

	}
}