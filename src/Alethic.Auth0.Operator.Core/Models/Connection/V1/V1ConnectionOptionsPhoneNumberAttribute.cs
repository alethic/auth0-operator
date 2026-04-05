using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    public class V1ConnectionOptionsPhoneNumberAttribute
    {

        [JsonPropertyName("signup")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V1ConnectionOptionsPhoneNumberSignup? Signup { get; set; }


    }

}
