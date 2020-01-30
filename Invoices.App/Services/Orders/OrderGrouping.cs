using Hangfire;
using Invoices.App.Models;
using Invoices.Data.Models;
using System;

namespace Invoices.App.Services.Orders
{
	public class OrderGrouping
	{

		private readonly TimeSpan _delay = TimeSpan.FromSeconds(15);

		private readonly InvoiceTracker _tracker;

		public OrderGrouping(InvoiceTracker tracker)
		{
			_tracker = tracker;
		}

		public void AddOrder(CreateInvoiceModel model, Order order)
		{
			// No orders for the given profile.
			if (!_tracker.HasTracking(model.UserId))
			{
				BackgroundJob.Enqueue<CreateInvoiceJob>(job => job.CreateInvoice(model, order));

				// Start a delayed background job to check for order changes.
				BackgroundJob.Schedule(() => CheckChange(model.UserId), _delay);
			} else
			{
				// Start tracking orders for the given profile.
				BackgroundJob.Enqueue<CreateInvoiceJob>(job => job.UpdateInvoice(model.UserId, order));
			}
		}

		public void CheckChange(string userId)
		{
			// Difference between now and the last order creation.
			TimeSpan delta = DateTime.Now.Subtract(_tracker.LastChange(userId));

			if (delta.CompareTo(_delay) < 0)
			{
				// The time to delay before checking again.
				TimeSpan newDelay = _delay.Subtract(delta);

				// Schedule a new task 5 minutes from the last received order.
				BackgroundJob.Schedule(() => CheckChange(userId), newDelay);
			} else
			{
				BackgroundJob.Enqueue<CreateInvoiceJob>(job => job.FinaliseInvoice(userId));
			}
		}
	}
}