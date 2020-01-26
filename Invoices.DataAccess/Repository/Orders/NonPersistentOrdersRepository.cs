using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Invoices.DataAccess.Repository.Orders
{
	public class NonPersistentOrdersRepository : NonPersistentRepository<Order>, IOrdersRepository
	{
		public NonPersistentOrdersRepository(List<Order> elements) : base(elements)
		{
		}

		public Task<IEnumerable<Order>> GetAll()
		{
			return Task.FromResult(_elements.AsEnumerable());
		}

		public Task<IEnumerable<Order>> GetOrdersAfter(DateTime dateTime)
		{
			return Task.FromResult(_elements
				.Where(o => o.PurchaseDateTime.CompareTo(dateTime) > 0)
				.OrderByDescending(o => o.PurchaseDateTime)
				.AsEnumerable());
		}
	}
}