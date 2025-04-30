namespace Tixora.Models
{
    public class booking
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int TicketQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public string PaymentStatus { get; set; } = "Pending"; // Paid, Failed, Refunded
        public string StripeSessionId { get; set; } = string.Empty;
    }
}
