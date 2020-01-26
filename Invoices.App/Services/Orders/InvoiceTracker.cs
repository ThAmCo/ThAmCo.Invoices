using Invoices.Data.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Invoices.App.Services.Orders
{

	public class InvoiceTracker
	{

		private readonly ConcurrentDictionary<int, Invoice> _invoices = new ConcurrentDictionary<int, Invoice>();

		private readonly ConcurrentDictionary<int, DateTime> _lastChange = new ConcurrentDictionary<int, DateTime>();

		public void Track(int profileId, Invoice invoice)
		{
			_invoices.TryAdd(profileId, invoice);

			_lastChange[profileId] = DateTime.Now;
		}

		public void Update(int profileId, Order order)
		{
			_invoices.TryGetValue(profileId, out Invoice invoice);

			order.InvoiceId = invoice.Id;
			order.Invoice = invoice;

			invoice.Orders.Add(order);

			_invoices[profileId] = invoice;
			_lastChange[profileId] = DateTime.Now;
		}

		public void StopTracking(int profileId)
		{
			_invoices.TryRemove(profileId, out _);
			_lastChange.TryRemove(profileId, out _);
		}

		public DateTime LastChange(int profileId)
		{
			if (!_lastChange.TryGetValue(profileId, out DateTime lastChange))
			{
				throw new KeyNotFoundException();
			}

			return lastChange;
		}

		public bool HasTracking(int profileId)
		{
			return _invoices.ContainsKey(profileId);
		}

		public Invoice GetInvoice(int profileId)
		{
			if (!_invoices.TryGetValue(profileId, out Invoice invoice))
			{
				throw new KeyNotFoundException();
			}

			return invoice;
		}

	}
}