using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionTotpSms
{

    [JsonPropertyName("length")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Length { get; set; }

    [JsonPropertyName("time_step")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TimeStep { get; set; }

}
