namespace sharecare_backend.Models.Payment
{
    public class FreePaymentEntity : PaymentTypeInterface
    {
        public int Id { get; set; }
        public PaymentTypeEnum Type { get; set; }
    }
}
