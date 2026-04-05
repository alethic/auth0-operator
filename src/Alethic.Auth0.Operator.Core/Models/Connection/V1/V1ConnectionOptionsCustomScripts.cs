using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public class V1ConnectionOptionsCustomScripts
    {

        [JsonPropertyName("login")]
        public string? Login { get; set; }

        [JsonPropertyName("get_user")]
        public string? GetUser { get; set; }

        [JsonPropertyName("delete")]
        public string? Delete { get; set; }

        [JsonPropertyName("change_password")]
        public string? ChangePassword { get; set; }

        [JsonPropertyName("verify")]
        public string? Verify { get; set; }

        [JsonPropertyName("create")]
        public string? Create { get; set; }

        [JsonPropertyName("change_username")]
        public string? ChangeUsername { get; set; }

        [JsonPropertyName("change_email")]
        public string? ChangeEmail { get; set; }

        [JsonPropertyName("change_phone_number")]
        public string? ChangePhoneNumber { get; set; }

    }

}