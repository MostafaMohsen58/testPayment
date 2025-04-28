using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace Tixora.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<ActionResult> Charge([FromBody] PaymentRequest model)
        //{
        //    var options = new PaymentIntentCreateOptions
        //    {
        //        Amount = 5000, // Amount in cents
        //        Currency = "usd",
        //        PaymentMethod = model.PaymentMethodId,
        //        ConfirmationMethod = "manual",
        //        Confirm = true,
        //    };

        //    var service = new PaymentIntentService();
        //    PaymentIntent intent = await service.CreateAsync(options);

        //    return Json(new { message = "Payment successful!" });
        //}

        //public class PaymentRequest
        //{
        //    public string PaymentMethodId { get; set; }
        //}
        //    public ActionResult CreateCheckoutSession()
        //    {
        //        //StripeConfiguration.ApiKey = "sk_test_51OBOHGLO7Fi7FPKNjXAOSdW2yvC9L42N4iMjWU2mFKlN0njhcgVwv7Cf1TmRBpMLNmrWA0etYGpWgNKLAp0yjHXu00dF9ZWKRl"; // Replace with your Stripe secret key

        //        //var options = new SessionCreateOptions
        //        //{
        //        //    PaymentMethodTypes = new List<string> { "card" },
        //        //    LineItems = new List<SessionLineItemOptions>
        //        //    {
        //        //        new SessionLineItemOptions
        //        //        {
        //        //            PriceData = new SessionLineItemPriceDataOptions
        //        //            {
        //        //                UnitAmount = 1500, // $15.00 (amount in cents)
        //        //                Currency = "usd",
        //        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //        //                {
        //        //                    Name = "Concert Ticket",
        //        //                },
        //        //            },
        //        //            Quantity = 1,
        //        //        },
        //        //    },
        //        //    Mode = "payment",
        //        //    SuccessUrl = "https://localhost:7076/Payment/Success",
        //        //    CancelUrl = "https://localhost:7076/Payment/Cancel",
        //        //};

        //        //var service = new SessionService();
        //        //Session session = service.Create(options);

        //        //return Redirect(session.Url); // Stripe-hosted checkout page
        //        var options = new SessionCreateOptions
        //        {
        //            PaymentMethodTypes = new List<string> { "card" },
        //            LineItems = new List<SessionLineItemOptions>
        //{
        //    new SessionLineItemOptions
        //    {
        //        PriceData = new SessionLineItemPriceDataOptions
        //        {
        //            UnitAmount = 1500,
        //            Currency = "usd",
        //            ProductData = new SessionLineItemPriceDataProductDataOptions
        //            {
        //                Name = "Concert Ticket",
        //            },
        //        },
        //        Quantity = 1,
        //    },
        //},
        //            Mode = "payment",
        //            SuccessUrl = "https://localhost:7076/Payment/Success?session_id={CHECKOUT_SESSION_ID}",
        //            CancelUrl = "https://localhost:7076/Payment/Cancel",
        //            Metadata = new Dictionary<string, string>
        //{
        //    { "UserId", "123" },
        //    { "Note", "First Stripe Payment" }
        //}
        //        };

        //    }
        public ActionResult CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 1500,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Concert Ticket",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "https://localhost:7076/Payment/Success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://localhost:7076/Payment/Cancel",
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", "123" },
                    { "Note", "First Stripe Payment" }
                }
            };

            var service = new SessionService();
            Session session = service.Create(options);

            // Redirect the user to Stripe Checkout page
            return Redirect(session.Url);
        }

        public ActionResult Success()
        {
            return View("Success");
        }

        public ActionResult Cancel()
        {
            return Content("❌ Payment cancelled.");
        }

    }
}
