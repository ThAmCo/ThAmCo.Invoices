using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Invoices.Data.Models;
using Invoices.App.Models;
using Invoices.DataAccess;
using Invoices.App.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		[HttpPost("postorder")]
		public async Task<IActionResult> PostOrder([FromBody, Bind("UserId,Address,Name,Price,ProductName,Email,PurchaseDateTime")] PostOrderRequest request)
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

			await _unitOfWork.Commit();

			var model = CreateInvoiceModel.FromPostOrderRequest(request);
			_orderGrouping.AddOrder(model, order);

			return Ok();
		}
	}
}
