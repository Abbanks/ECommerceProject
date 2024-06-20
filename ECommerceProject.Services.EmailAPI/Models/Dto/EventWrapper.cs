using Newtonsoft.Json;

namespace ECommerceProject.Services.EmailAPI.Models.Dto
{
    public class EventWrapper<T>
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("detail-type")]
        public string DetailType { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("resources")]
        public List<string> Resources { get; set; }

        [JsonProperty("detail")]
        public T Detail { get; set; }
    }
}
