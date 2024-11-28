using System.Text.Json.Serialization;

namespace QuickMed.ViewModels
{
    public class BrandVM
    {
        [JsonPropertyName("value")]
        public string value { get; set; }
        [JsonPropertyName("text")]
        public string text { get; set; }
    }

    public class DoseVM
    {
        [JsonPropertyName("value")]
        public string value { get; set; }
        [JsonPropertyName("text")]
        public string text { get; set; }
    }

    public class DurationVM
    {

        [JsonPropertyName("value")]
        public string value { get; set; }
        [JsonPropertyName("text")]
        public string text { get; set; }
    }

    public class InstructionVM
    {
        [JsonPropertyName("value")]
        public string value { get; set; }
        [JsonPropertyName("text")]
        public string text { get; set; }
    }

    public class TreatmentPopVM
    {
        public int index { get; set; }
        public string? tempId { get; set; }
        public BrandVM brand { get; set; }
        public DoseVM dose { get; set; }
        public DurationVM duration { get; set; }
        public InstructionVM instruction { get; set; }
    }

}
