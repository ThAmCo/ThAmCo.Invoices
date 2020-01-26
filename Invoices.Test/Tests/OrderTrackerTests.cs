using Invoices.App.Services.Orders;
using Invoices.Data.Models;
using System;
using Xunit;

namespace Invoices.Test.Tests
{
	public class OrderTrackerTests
	{

		[Fact]
		public void TrackTest()
		{
			var order = new Order { ProductName = "colander", Address = "64 Zoo Lane", Name = "Spaghetti Gonsalves", Price = 0.99, PurchaseDateTime = new DateTime(2015, 11, 26) };

			var tracker = new InvoiceTracker();

			//tracker.Track(1, order);

			Assert.True(tracker.HasTracking(1));

			//tracker.
		}

	}
}