using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Invoices.DataAccess.Repository.Orders
{
	public interface IOrdersRepository : IRepository<int, Order>
	{

		Task<IEnumerable<Order>> GetAll();

		Task<IEnumerable<Order>> GetOrdersAfter(DateTime dateTime);

	}
}