using sharecare_backend.Models.Location;
using sharecare_backend.Models.Payment;
using sharecare_backend.Models.Time;
using sharecare_backend.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace sharecare_backend.Models.Problem
{
    public class ProblemDBEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TypeJson { get; set; }
        public string TimeJson { get; set; }
        public bool IsLocationBound { get; set; }
        public string LocationJson { get; set; }
        public string PaymentJson { get; set; }
        public int[] ProvidersId { get; set; }
        public int[] SearchersId { get; set; }

        public ProblemEntity ToNormalProblem()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // 1. Cleanly deserialize the non-interface objects
            LocationEntity location = string.IsNullOrWhiteSpace(LocationJson)
                ? null
                : JsonSerializer.Deserialize<LocationEntity>(LocationJson, options);

            ProblemTypeEnum type = string.IsNullOrWhiteSpace(TypeJson)
                ? default
                : JsonSerializer.Deserialize<ProblemTypeEnum>(TypeJson, options);

            // 2. BULLETPROOF TIME INTERFACE PARSING (No Custom Converter Needed)
            TimeTypeInterface time = null;
            if (!string.IsNullOrWhiteSpace(TimeJson))
            {
                using (JsonDocument doc = JsonDocument.Parse(TimeJson))
                {
                    var root = doc.RootElement;
                    int typeValue = 0; // Default fallback

                    // Check your type discriminator field
                    if (root.TryGetProperty("type", out var typeProp) || root.TryGetProperty("Type", out typeProp))
                    {
                        typeValue = typeProp.GetInt32();
                    }

                    if (typeValue == 1 || root.TryGetProperty("startTime", out _) || root.TryGetProperty("StartTime", out _))
                    {
                        time = JsonSerializer.Deserialize<RangeTimeEntity>(TimeJson, options);
                    }
                    else
                    {
                        time = JsonSerializer.Deserialize<FixedTimeEntity>(TimeJson, options);
                    }
                }
            }

            // 3. BULLETPROOF PAYMENT INTERFACE PARSING
            PaymentTypeInterface payment = null;
            if (!string.IsNullOrWhiteSpace(PaymentJson))
            {
                // Directly parse to your concrete entity
                payment = JsonSerializer.Deserialize<MoneyPaymentEntity>(PaymentJson, options);
            }

            // 4. Map everything safely to the Domain Entity
            ProblemEntity problem = new ProblemEntity
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Type = type,
                Time = time,
                IsLocationBound = IsLocationBound,
                Location = location,
                Payment = payment,
                ProvidersId = ProvidersId?.ToList() ?? new List<int>(),
                SearchersId = SearchersId?.ToList() ?? new List<int>()
            };

            return problem;
        }
    }
}