using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Stripe.V2;
using System.Threading.Tasks;
using Tixora.Models;

namespace Tixora.Controllers
{
    public class PaymentController : Controller
    {
        private IConfiguration _config;
        private readonly AppDbContext _context;

        public PaymentController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;

            StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
        }
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
        [HttpPost]
        public async Task<ActionResult> CreateCheckoutSession(int bookingId)
        {
            var booking = _context.Bookings.Find(bookingId);
            if (booking == null)
            {
                return NotFound("Booking not found.");
            }
            var eventDetails = _context.Events.Find(booking.EventId);
            if (eventDetails == null)
            {
                return NotFound("Event not found.");
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(booking.TotalAmount * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = eventDetails.Title,
                                Description = $"Ticket for {eventDetails.Title}",
                            },
                        },
                        Quantity = booking.TicketQuantity,
                    },
                },
                Mode = "payment",
                // SuccessUrl = "https://localhost:7076/Payment/Success?session_id={CHECKOUT_SESSION_ID}",
                //CancelUrl = "https://localhost:7076/Payment/Cancel",
                SuccessUrl = $"{_config["BaseUrl"]}/Payment/Success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_config["BaseUrl"]}/Payment/Cancel",
                Metadata = new Dictionary<string, string>
                {
                    { "UserId", booking.UserId },
                    { "BookingId", bookingId.ToString() }

                }
            };

            var service = new SessionService();
            Session session = service.Create(options);

            // Redirect the user to Stripe Checkout page
            //return Redirect(session.Url);
            booking.StripeSessionId = session.Id;
            await _context.SaveChangesAsync();
            return Json(new { url = session.Url });
        }

        public async Task<ActionResult> Success(string session_id)
        {
            var service = new SessionService();
            var session = await service.GetAsync(session_id);

            if (session.PaymentStatus == "paid")
            {
                // Payment was successful
                // You can retrieve the booking ID from the session metadata
                var bookingId = session.Metadata["BookingId"];
                // Update your database or perform any other actions here
                return Content("✅ Payment successful! Booking ID: " + bookingId);
            }
            else
            {
                return Content("❌ Payment failed.");
            }
        }

        public ActionResult Cancel()
        {
            return Content("❌ Payment cancelled.");
        }

    }
}
