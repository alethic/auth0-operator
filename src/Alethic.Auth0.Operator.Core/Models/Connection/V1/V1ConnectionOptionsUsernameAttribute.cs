using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public record V1ConnectionOptionsUsernameAttribute
    {

        [JsonPropertyName("identifier")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsAttributeIdentifier? Identifier { get; set; }

        [JsonPropertyName("profile_required")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ProfileRequired { get; set; }

        [JsonPropertyName("signup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsUsernameSignup? Signup { get; set; }

        [JsonPropertyName("validation")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsAttributeValidation? Validation { get; set; }

    }

}