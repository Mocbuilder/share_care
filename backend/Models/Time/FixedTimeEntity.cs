namespace sharecare_backend.Models.Time
{
    public class FixedTimeEntity : TimeTypeInterface
    {
        public int Id { get; set; }
        public TimeTypeEnum Type { get; set; }
        public DateTime Time { get; set; }
    }
}
