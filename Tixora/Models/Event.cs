namespace Tixora.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal TicketPrice { get; set; }
        public int AvailableTickets { get; set; }
    }
}
