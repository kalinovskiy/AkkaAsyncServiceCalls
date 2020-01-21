using Newtonsoft.Json;

namespace AkkaAsyncServiceCalls.Common.Dtos
{
    public class DtoPerson
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
