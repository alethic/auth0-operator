using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientMobileAndroid
{

    [JsonPropertyName("app_package_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AppPackageName { get; set; }

    [JsonPropertyName("sha256_cert_fingerprints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Sha256CertFingerprints { get; set; }

}
