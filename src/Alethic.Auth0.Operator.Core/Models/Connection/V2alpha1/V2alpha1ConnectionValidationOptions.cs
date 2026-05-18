using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    public record V2alpha1ConnectionValidationOptions
    {



        [Nullable, Optional]
        [JsonPropertyName("username")]
        public ConnectionUsernameValidationOptions? Username { get; set; }

        [JsonExtensionData]
        public ReadOnlyAdditionalProperties AdditionalProperties { get; private set; } = new();

    }

}