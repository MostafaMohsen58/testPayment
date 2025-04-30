using Tixora.Models;

namespace Tixora.Data
{
    public class DbInit()
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Events.Any())
            {
                var events = new List<Event>
                {
                    new Event
                    {
                        Title = "Concert",
                        Description = "Live concert featuring popular bands.",
                        Date = DateTime.Now.AddDays(30),
                        TicketPrice = 50.00m,
                        AvailableTickets = 100
                    },
                    new Event
                    {
                        Title = "Art Exhibition",
                        Description = "Showcasing local artists.",
                        Date = DateTime.Now.AddDays(60),
                        TicketPrice = 20.00m,
                        AvailableTickets = 200
                    }
                };
                context.Events.AddRange(events);
                context.SaveChanges();
            }

            if (!context.Bookings.Any())
            {
                var bookings = new List<booking>
                {
                    new booking
                    {
                        EventId = 1,
                        TicketQuantity = 2,
                        TotalAmount = 100.00m,
                        PaymentStatus = "Pending"
                    },
                    new booking
                    {
                        EventId = 2,
                        TicketQuantity = 1,
                        TotalAmount = 150.00m,
                        PaymentStatus = "Paid",
                        StripeSessionId = "stripe-session-id-123"
                    }
                };
                context.Bookings.AddRange(bookings);
                context.SaveChanges();
            }
        }
        
    }
}
