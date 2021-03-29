using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace pelsoft.auth.models
{
    public class AuthenticationRequest
    {
        [Required]
        public string grant_type { get; set; }
        public string client_secret { get; set; }
        public string refresh_token { get; set; }
        public string security_provider { get; set; }
        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalData { get; set; }
    }
}
