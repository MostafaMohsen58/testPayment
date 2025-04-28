namespace Tixora.ViewModel
{
    public class CheckoutViewModel
    {
        public int EventId { get; set; }
        public int TicketQuantity { get; set; }
        public long TotalAmount { get; set; } // in cents
        public string PublishableKey { get; set; }
    }
}
