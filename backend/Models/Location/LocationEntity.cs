namespace sharecare_backend.Models.Location
{
    public class LocationEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CorLat { get; set; }
        public string CorLon { get; set; }
    }
}
