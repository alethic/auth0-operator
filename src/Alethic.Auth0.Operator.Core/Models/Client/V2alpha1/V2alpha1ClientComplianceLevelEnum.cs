using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ClientComplianceLevelEnum
{

    [JsonStringEnumMemberName("none")]
    None,

    [JsonStringEnumMemberName("fapi1_adv_pkj_par")]
    Fapi1AdvPkjPar,

    [JsonStringEnumMemberName("fapi1_adv_mtls_par")]
    Fapi1AdvMtlsPar,

    [JsonStringEnumMemberName("fapi2_sp_pkj_mtls")]
    Fapi2SpPkjMtls,

    [JsonStringEnumMemberName("fapi2_sp_mtls_mtls")]
    Fapi2SpMtlsMtls

}
