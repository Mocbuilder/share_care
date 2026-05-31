namespace sharecare_backend.Models.Payment
{
    public class CustomPaymentEntity : PaymentTypeInterface
    {
        public int Id { get; set; }
        public PaymentTypeEnum Type { get; set; }
        public string CustomText { get; set; }
    }
}
