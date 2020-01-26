using Invoices.Data.Models;
using Invoices.DataAccess;
using System;
using System.Threading.Tasks;

namespace Invoices.App.Services.Orders
{
	public class CreateInvoiceJob
	{

		private readonly InvoiceTracker _tracker;

		private readonly IUnitOfWork _unitOfWork;

		public CreateInvoiceJob(IUnitOfWork unitOfWork, InvoiceTracker tracker)
		{
			_unitOfWork = unitOfWork;
			_tracker = tracker;
		}

		public async Task CreateInvoice(Profile profile, Order initialOrder)
		{
			var invoice = new Invoice
			{
				ProfileId = profile.Id,
				State = InvoiceState.Unfinished,
				Address = profile.Address,
				Email = profile.Email,
				Name = profile.Name
			};

			initialOrder.InvoiceId = invoice.Id;
			initialOrder.Invoice = invoice;
			_unitOfWork.Orders.Update(initialOrder);

			_unitOfWork.Invoices.Create(invoice);
			await _unitOfWork.Commit();

			_tracker.Track(profile.Id, invoice);
		}

		public async Task FinaliseInvoice(int profileId)
		{
			var invoice = _tracker.GetInvoice(profileId);

			invoice.InvoicedAt = DateTime.Now;
			invoice.State = InvoiceState.Created;

			_unitOfWork.Invoices.Update(invoice);
			await _unitOfWork.Commit();

			_tracker.StopTracking(profileId);
		}

		public async Task UpdateInvoice(int profileId, Order order)
		{
			var invoice = _tracker.GetInvoice(profileId);

			order.InvoiceId = invoice.Id;
			order.Invoice = invoice;

			_unitOfWork.Orders.Update(order);
			await _unitOfWork.Commit();

			_tracker.Update(profileId, order);
		}

	}
}