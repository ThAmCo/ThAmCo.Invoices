using System.Threading.Tasks;

namespace Invoices.App.Services.Email
{
	public interface IEmailService
	{

		public Task<bool> SendHtmlEmail(string from, string fromName, string to, string toName, string subject, string content);

	}
}
