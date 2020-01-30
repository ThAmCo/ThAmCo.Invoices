using Invoices.App.Models;
using Invoices.App.Services.Email;
using Invoices.Data.Models;
using Invoices.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.App.Controllers
{
	public class InvoicesController : Controller
	{

		private readonly IEmailService _emailService;

		private readonly IUnitOfWork _unitOfWork;

		private readonly ICompositeViewEngine _viewEngine;

		public InvoicesController(IUnitOfWork unitOfWork, IEmailService emailService, ICompositeViewEngine viewEngine)
		{
			_unitOfWork = unitOfWork;
			_emailService = emailService;
			_viewEngine = viewEngine;
		}

		[Authorize(Policy = "StaffOnly")]
		public async Task<IActionResult> Index()
		{
			var invoices = await _unitOfWork.Invoices.GetAll();

			return View(new InvoicesIndexViewModel { Invoices = invoices, RedirectState = InvoicesIndexRedirectState.NO_MESSAGE });
		}

		[Authorize]
		public async Task<IActionResult> List()
		{
			var userId = User.FindFirst("sub").Value;
			var invoices = await _unitOfWork.Invoices.ForUser(userId);

			return View(invoices);
		}

		[Authorize]
		public async Task<IActionResult> Details(int id)
		{
			var userId = User.FindFirst("sub").Value;
			var userInvoices = await _unitOfWork.Invoices.ForUser(userId);

			var invoice = userInvoices.First(i => i.Id == id);

			if (invoice == null)
			{
				return NotFound();
			}

			return View(invoice);
		}

		[Authorize(Policy = "StaffOnly")]
		public async Task<IActionResult> Approve(int id)
		{
			return View(await _unitOfWork.Invoices.Get(id));
		}

		[Authorize(Policy = "StaffOnly")]
		public async Task<IActionResult> ConfirmApprove(int id)
		{
			var invoices = await _unitOfWork.Invoices.GetAll();
			var localInvoice = invoices.First(i => i.Id == id);

			var indexViewModel = new InvoicesIndexViewModel { Invoices = invoices };

			if (!localInvoice.State.Equals(InvoiceState.Sent))
			{
				indexViewModel.RedirectState = InvoicesIndexRedirectState.ALREADY_APPROVED;
			}
			else
			{
				try
				{
					_unitOfWork.Invoices.Update(localInvoice);
					await _unitOfWork.Commit();
				} catch (DbException)
				{
					indexViewModel.RedirectState = InvoicesIndexRedirectState.PERSISTENCE_FAILURE;
				}

				string emailContent = await InvoiceViewAsString(localInvoice);

				var success = await _emailService.SendHtmlEmail("invoicing@thamco.com", "Invoicing",
					localInvoice.Email, localInvoice.Name,
					"Your order has been invoiced!", emailContent);

				if (!success)
				{
					indexViewModel.RedirectState = InvoicesIndexRedirectState.EMAIL_FAILED;
				} else
				{
					indexViewModel.RedirectState = InvoicesIndexRedirectState.APPROVAL_SUCCESSFUL;
					localInvoice.State = InvoiceState.Sent;
				}
			}

			return View(nameof(Index), indexViewModel);
		}

		private async Task<string> InvoiceViewAsString(Invoice invoice)
		{
			ViewData.Model = invoice;

			using var writer = new StringWriter();

			ViewEngineResult viewResult = _viewEngine.FindView(ControllerContext, "_InvoiceDetails", false);
			ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, writer, new HtmlHelperOptions());

			await viewResult.View.RenderAsync(viewContext);

			return "<center>" + writer.GetStringBuilder().ToString() + "</center>";
		}
	}
}