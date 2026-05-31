namespace sharecare_backend.Models.Payment
{
    public class MoneyPaymentEntity : PaymentTypeInterface
    {
        public int Id { get; set; }
        public PaymentTypeEnum Type { get; set; }
        public decimal Amount { get; set; }
    }
}
