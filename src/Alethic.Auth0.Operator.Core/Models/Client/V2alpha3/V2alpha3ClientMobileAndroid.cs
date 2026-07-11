using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

public record V2alpha3ClientMobileAndroid
{

    [JsonPropertyName("appPackageName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AppPackageName { get; set; }

    [JsonPropertyName("sha256CertFingerprints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Sha256CertFingerprints { get; set; }

}
