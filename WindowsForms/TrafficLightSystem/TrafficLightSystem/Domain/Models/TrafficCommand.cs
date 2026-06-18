namespace TrafficLightSystem.Domain.Models
{
    public class TrafficCommand
    {
        public string Type { get; set; }
        public string Payload { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Payload)
                ? Type
                : $"{Type},{Payload}";
        }
    }
}