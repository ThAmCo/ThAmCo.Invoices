using Invoices.App.Models;
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

		public async Task CreateInvoice(CreateInvoiceModel model, Order initialOrder)
		{
			var invoice = new Invoice
			{
				UserId = model.UserId,
				State = InvoiceState.Unfinished,
				Address = model.Address,
				Email = model.Email,
				Name = model.Name
			};

			initialOrder.InvoiceId = invoice.Id;
			initialOrder.Invoice = invoice;
			_unitOfWork.Orders.Update(initialOrder);

			_unitOfWork.Invoices.Create(invoice);
			await _unitOfWork.Commit();

			_tracker.Track(model.UserId, invoice);
		}

		public async Task FinaliseInvoice(string userId)
		{
			var invoice = _tracker.GetInvoice(userId);

			invoice.InvoicedAt = DateTime.Now;
			invoice.State = InvoiceState.Created;

			_unitOfWork.Invoices.Update(invoice);
			await _unitOfWork.Commit();

			_tracker.StopTracking(userId);
		}

		public async Task UpdateInvoice(string userId, Order order)
		{
			var invoice = _tracker.GetInvoice(userId);

			order.InvoiceId = invoice.Id;
			order.Invoice = invoice;

			_unitOfWork.Orders.Update(order);
			await _unitOfWork.Commit();

			_tracker.Update(userId, order);
		}

	}
}