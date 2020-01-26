using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Invoices.App.Services.Email
{
	public class SendGridEmailService : IEmailService
	{

		public async Task<bool> SendHtmlEmail(string from, string fromName, string to, string toName, string subject, string content)
		{
			var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
			var client = new SendGridClient(apiKey);
			var fromEmail = new EmailAddress(from, fromName);
			var toEmail = new EmailAddress(to, toName);
			var msg = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, "", content);

			var response = await client.SendEmailAsync(msg);

			return response.StatusCode.Equals(HttpStatusCode.Accepted) || response.StatusCode.Equals(HttpStatusCode.OK);
		}

	}
}