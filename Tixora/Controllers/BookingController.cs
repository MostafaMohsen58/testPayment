using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tixora.Models;

namespace Tixora.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;
        public BookingController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var bookings = _context.Bookings.ToList();
            return View(bookings);
        }
        public IActionResult Create()
        {
            var events = _context.Events.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.Title} (${e.TicketPrice})"
            }).ToList();
            ViewBag.Events = events;
            return View();
        }
        [HttpPost]
        public IActionResult Create(booking booking)
        {
            if (ModelState.IsValid)
            {
                var selectedEvent = _context.Events.Find(booking.EventId);
                if (selectedEvent != null)
                {
                    booking.TotalAmount = selectedEvent.TicketPrice * booking.TicketQuantity;
                    booking.BookingDate = DateTime.Now;
                    booking.PaymentStatus = "Pending"; // Initial status

                    _context.Bookings.Add(booking);
                    _context.SaveChanges();
                    return RedirectToAction("StripeCheckout", new { bookingId = booking.Id });
                }
            }
            ViewBag.Events = _context.Events.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = $"{e.Title} (${e.TicketPrice})"
            }).ToList();
            return View(booking);
        }

        [HttpPost]
        //return RedirectToAction("StripeCheckout", "Payment", new { bookingId = 123 });

        public IActionResult ConfirmBooking(int eventId, int ticketQuantity)
        {
            var booking = new booking
            {
                EventId = eventId,
                TicketQuantity = ticketQuantity,
                TotalAmount = 50 * ticketQuantity
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("StripeCheckout", "Payment", new { bookingId = booking.Id });
        }
    }
}
