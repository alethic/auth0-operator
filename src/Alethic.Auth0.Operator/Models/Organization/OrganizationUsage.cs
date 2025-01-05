﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Models.Organization
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrganizationUsage
    {

        [JsonStringEnumMemberName("deny")]
        Deny,

        [JsonStringEnumMemberName("allow")]
        Allow,

        [JsonStringEnumMemberName("require")]
        Require

    }

}
