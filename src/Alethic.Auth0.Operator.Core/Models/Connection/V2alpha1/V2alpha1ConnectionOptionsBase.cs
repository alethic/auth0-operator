using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    public record V2alpha1ConnectionOptionsBase
    {

        [JsonExtensionData]
        public Dictionary<string, object?> AdditionalProperties { get; set; } = new();

    }

}
