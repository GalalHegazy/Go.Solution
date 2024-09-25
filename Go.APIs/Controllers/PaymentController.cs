using Go.APIs.Errors;
using Go.Core.Entities.Basket;
using Go.Core.Entities.Order_Aggregate;
using Go.Core.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Go.APIs.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;
        private const string endpointSecret = "whsec_7925b0f9770b09a3a785c47fb71c7276078c97de33ade46f8c8295c7c771e9c3";


        public PaymentController(IPaymentService paymentService
                                ,ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        //[Authorize]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponce), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket?>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId);

            if (basket is null) return BadRequest(new APIResponce(400, "An Error With Your Basket"));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> webHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,Request.Headers["Stripe-Signature"], endpointSecret);


            // Handle the event

            var paymentIntent = (PaymentIntent) stripeEvent.Data.Object;

            Order? order;

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    order= await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);
                    _logger.LogInformation("Order Is Succeeded {0}", order?.PaymentIntentId);
                    _logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
                    break;
                case Events.PaymentIntentPaymentFailed:
                    order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
                    _logger.LogInformation("Order Is Failed {0}", order?.PaymentIntentId);
                    _logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
                    break;

            }
            return Ok();
        }
    }
}
