﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ClientApplicationType
    {

        [JsonStringEnumMemberName("box")]
        Box,

        [JsonStringEnumMemberName("cloudbees")]
        Cloudbees,

        [JsonStringEnumMemberName("concur")]
        Concur,

        [JsonStringEnumMemberName("dropbox")]
        Dropbox,

        [JsonStringEnumMemberName("echosign")]
        Echosign,

        [JsonStringEnumMemberName("egnyte")]
        Egnyte,

        [JsonStringEnumMemberName("mscrm")]
        MsCrm,

        [JsonStringEnumMemberName("native")]
        Native,

        [JsonStringEnumMemberName("newrelic")]
        NewRelic,

        [JsonStringEnumMemberName("non_interactive")]
        NonInteractive,

        [JsonStringEnumMemberName("office365")]
        Office365,

        [JsonStringEnumMemberName("regular_web")]
        RegularWeb,

        [JsonStringEnumMemberName("rms")]
        Rms,

        [JsonStringEnumMemberName("salesforce")]
        Salesforce,

        [JsonStringEnumMemberName("sentry")]
        Sentry,

        [JsonStringEnumMemberName("sharepoint")]
        SharePoint,

        [JsonStringEnumMemberName("slack")]
        Slack,

        [JsonStringEnumMemberName("springcm")]
        SpringCm,

        [JsonStringEnumMemberName("spa")]
        Spa,

        [JsonStringEnumMemberName("zendesk")]
        Zendesk,

        [JsonStringEnumMemberName("zoom")]
        Zoom

    }

}
