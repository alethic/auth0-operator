﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ComplianceLevel
    {

        [JsonStringEnumMemberName("none")]
        NONE,

        [JsonStringEnumMemberName("fapi1_adv_pkj_par")]
        FAPI1_ADV_PKJ_PAR,

        [JsonStringEnumMemberName("fapi1_adv_mtls_par")]
        FAPI1_ADV_MTLS_PAR

    }

}
