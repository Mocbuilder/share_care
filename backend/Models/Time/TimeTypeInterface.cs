using System.Text.Json.Serialization;

namespace sharecare_backend.Models.Time
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
    [JsonDerivedType(typeof(FixedTimeEntity), typeDiscriminator: "fixed")]
    [JsonDerivedType(typeof(RangeTimeEntity), typeDiscriminator: "range")]
    public interface TimeTypeInterface
    {
        public int Id { get; set; }
        public TimeTypeEnum Type { get; set; }
        
    }
}
