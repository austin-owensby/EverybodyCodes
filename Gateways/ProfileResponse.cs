using System.Text.Json.Serialization;

namespace EverybodyCodes.Gateways
{
    public class ProfileResponse
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public int Level { get; set; }
        public int Seed { get; set; }
        public long PenaltyUntil { get; set; }
        public long ServerTime { get; set; }
        public Badge Badges { get; set; } = new();
    }

    public class Badge {
        [JsonPropertyName("2024")]
        public string Badge2024 { get; set;} = string.Empty;
    }
}