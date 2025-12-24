using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection
{

    public partial class V1ConnectionFind
    {

        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ConnectionId { get; set; }

    }

}
