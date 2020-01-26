using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Invoices.Data.Models;
using Invoices.App.Models;
using Invoices.DataAccess;
using Invoices.App.Services.Orders;

namespace Invoices.App.Controllers
{
    [Route("invoices/api")]
    [ApiController]
    public class InvoicesApiController : ControllerBase
    {

		private readonly OrderGrouping _orderGrouping;

		private readonly IUnitOfWork _unitOfWork;

        public InvoicesApiController(OrderGrouping orderGrouping, IUnitOfWork unitOfWork)
        {
			_orderGrouping = orderGrouping;
			_unitOfWork = unitOfWork;
		}

		[HttpPost("postorder")]
		public async Task<IActionResult> PostOrder([FromBody, Bind("ProfileId,Address,Name,Price,ProductName,Email,PurchaseDateTime")] PostOrderRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var order = new Order
			{
				Address = request.Address,
				Name = request.Name,
				Price = request.Price,
				ProductName = request.ProductName,
				PurchaseDateTime = request.PurchaseDateTime,
			};

			_unitOfWork.Orders.Update(order);

			var profile = request.GetProfile();
			_unitOfWork.Profiles.Update(profile);

			await _unitOfWork.Commit();

			_orderGrouping.AddOrder(profile, order);

			return Ok();
		}
	}
}
