using System;
using System.Threading.Tasks;

namespace Invoices.App.Services.Email
{
	public class FakeEmailService : IEmailService
	{
		public Task<bool> SendHtmlEmail(string from, string fromName, string to, string toName, string subject, string content)
		{
			Console.WriteLine(from + " to " + to + ", " + subject + ": \n" + content);

			return Task.FromResult(true);
		}
	}
}
