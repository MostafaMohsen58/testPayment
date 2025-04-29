using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var events = _context.Events.ToList();
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
                    booking.UserId = "user123";
                    _context.Bookings.Add(booking);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Events = _context.Events.ToList();
            return View(booking);
        }
    }
}
