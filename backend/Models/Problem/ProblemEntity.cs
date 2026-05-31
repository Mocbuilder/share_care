using sharecare_backend.Models.Location;
using sharecare_backend.Models.Payment;
using sharecare_backend.Models.Time;

namespace sharecare_backend.Models.Problem
{
    public class ProblemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProblemTypeEnum Type { get; set; }
        public TimeTypeInterface Time { get; set; }
        public bool IsLocationBound { get; set; }
        public LocationEntity Location { get; set; }
        public PaymentTypeInterface Payment { get; set; }
        public List<int> ProvidersId { get; set; }
        public List<int> SearchersId { get; set; }

        public ProblemDBEntity ToDBProblem()
        {
            return new ProblemDBEntity
            {
                Id = Id,
                Name = Name,
                Description = Description,
                TypeJson = System.Text.Json.JsonSerializer.Serialize(Type),
                TimeJson = System.Text.Json.JsonSerializer.Serialize(Time),
                IsLocationBound = IsLocationBound,
                LocationJson = System.Text.Json.JsonSerializer.Serialize(Location),
                PaymentJson = System.Text.Json.JsonSerializer.Serialize(Payment),
                ProvidersId = ProvidersId?.ToArray(),
                SearchersId = SearchersId?.ToArray()
            };
        }
    }
}