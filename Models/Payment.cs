namespace WebApplicationTest.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }  // Foreign key to Order
        public virtual Order Order { get; set; }

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string PaymentMethod { get; set; }  // e.g., Credit Card, PayPal
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

}
