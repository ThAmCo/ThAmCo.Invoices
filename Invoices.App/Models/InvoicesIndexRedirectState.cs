namespace Invoices.App.Models
{
	public class InvoicesIndexRedirectState
	{

		private readonly string _message;

		private readonly string _alertType;

		private InvoicesIndexRedirectState(string message, string alertType)
		{
			_message = message;
			_alertType = alertType;
		}

		public string GetMessage()
		{
			return _message;
		}

		public string GetAlertType()
		{
			return _alertType;
		}

		public static readonly InvoicesIndexRedirectState APPROVAL_SUCCESSFUL = new InvoicesIndexRedirectState("Invoice successfully approved!", "success");

		public static readonly InvoicesIndexRedirectState ALREADY_APPROVED = new InvoicesIndexRedirectState("That invoice has already been approved!", "danger");

		public static readonly InvoicesIndexRedirectState EMAIL_FAILED = new InvoicesIndexRedirectState("Failed to send invoice via email!", "danger");

		public static readonly InvoicesIndexRedirectState PERSISTENCE_FAILURE = new InvoicesIndexRedirectState("Failed to save Invoice!", "danger");

		public static readonly InvoicesIndexRedirectState NO_MESSAGE = new InvoicesIndexRedirectState("", "");

	}
}