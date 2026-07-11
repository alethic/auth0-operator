using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ClientComplianceLevelEnum
{

    [JsonStringEnumMemberName("none")]
    None,

    [JsonStringEnumMemberName("fapi1AdvPkjPar")]
    Fapi1AdvPkjPar,

    [JsonStringEnumMemberName("fapi1AdvMtlsPar")]
    Fapi1AdvMtlsPar,

    [JsonStringEnumMemberName("fapi2SpPkjMtls")]
    Fapi2SpPkjMtls,

    [JsonStringEnumMemberName("fapi2SpMtlsMtls")]
    Fapi2SpMtlsMtls

}
