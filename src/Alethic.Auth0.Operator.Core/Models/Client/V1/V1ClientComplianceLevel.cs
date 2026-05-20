using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ClientComplianceLevel
    {

        [JsonStringEnumMemberName("none")]
        None,

        [JsonStringEnumMemberName("fapi1_adv_pkj_par")]
        Fapi1AdvPkjPar,

        [JsonStringEnumMemberName("fapi1_adv_mtls_par")]
        Fapi1AdvMtlsPar

    }

}
