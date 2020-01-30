using Invoices.Data.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Invoices.App.Services.Orders
{

	public class InvoiceTracker
	{

		private readonly ConcurrentDictionary<string, Invoice> _invoices = new ConcurrentDictionary<string, Invoice>();

		private readonly ConcurrentDictionary<string, DateTime> _lastChange = new ConcurrentDictionary<string, DateTime>();

		public void Track(string userId, Invoice invoice)
		{
			_invoices.TryAdd(userId, invoice);

			_lastChange[userId] = DateTime.Now;
		}

		public void Update(string userId, Order order)
		{
			_invoices.TryGetValue(userId, out Invoice invoice);

			order.InvoiceId = invoice.Id;
			order.Invoice = invoice;

			invoice.Orders.Add(order);

			_invoices[userId] = invoice;
			_lastChange[userId] = DateTime.Now;
		}

		public void StopTracking(string userId)
		{
			_invoices.TryRemove(userId, out _);
			_lastChange.TryRemove(userId, out _);
		}

		public DateTime LastChange(string userId)
		{
			if (!_lastChange.TryGetValue(userId, out DateTime lastChange))
			{
				throw new KeyNotFoundException();
			}

			return lastChange;
		}

		public bool HasTracking(string userId)
		{
			return _invoices.ContainsKey(userId);
		}

		public Invoice GetInvoice(string userId)
		{
			if (!_invoices.TryGetValue(userId, out Invoice invoice))
			{
				throw new KeyNotFoundException();
			}

			return invoice;
		}

	}
}