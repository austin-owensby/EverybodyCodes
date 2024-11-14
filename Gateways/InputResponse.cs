using System.Text.Json.Serialization;

namespace EverybodyCodes.Gateways
{
    public class InputResponse
    {
        [JsonPropertyName("1")]
        public string PartOne { get; set; } = string.Empty;
        [JsonPropertyName("2")]
        public string PartTwo { get; set; } = string.Empty;
        [JsonPropertyName("3")]
        public string PartThree { get; set; } = string.Empty;
    }
}