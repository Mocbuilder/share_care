using sharecare_backend.Models.Time;
using System.Text.Json.Serialization;

namespace sharecare_backend.Models.Payment
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(FreePaymentEntity), typeDiscriminator: "free")]
    [JsonDerivedType(typeof(CustomPaymentEntity), typeDiscriminator: "custom")]
    [JsonDerivedType(typeof(MoneyPaymentEntity), typeDiscriminator: "money")]
    public interface PaymentTypeInterface
    {
        public int Id { get; }
        public PaymentTypeEnum Type { get; }
    }
}
