using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ClientAppTypeEnum
{

    [JsonStringEnumMemberName("native")]
    Native,

    [JsonStringEnumMemberName("spa")]
    Spa,

    [JsonStringEnumMemberName("regular_web")]
    RegularWeb,

    [JsonStringEnumMemberName("non_interactive")]
    NonInteractive,

    [JsonStringEnumMemberName("resource_server")]
    ResourceServer,

    [JsonStringEnumMemberName("express_configuration")]
    ExpressConfiguration,

    [JsonStringEnumMemberName("rms")]
    Rms,

    [JsonStringEnumMemberName("box")]
    Box,

    [JsonStringEnumMemberName("cloudbees")]
    Cloudbees,

    [JsonStringEnumMemberName("concur")]
    Concur,

    [JsonStringEnumMemberName("dropbox")]
    Dropbox,

    [JsonStringEnumMemberName("mscrm")]
    Mscrm,

    [JsonStringEnumMemberName("echosign")]
    Echosign,

    [JsonStringEnumMemberName("egnyte")]
    Egnyte,

    [JsonStringEnumMemberName("newrelic")]
    Newrelic,

    [JsonStringEnumMemberName("office365")]
    Office365,

    [JsonStringEnumMemberName("salesforce")]
    Salesforce,

    [JsonStringEnumMemberName("sentry")]
    Sentry,

    [JsonStringEnumMemberName("sharepoint")]
    Sharepoint,

    [JsonStringEnumMemberName("slack")]
    Slack,

    [JsonStringEnumMemberName("springcm")]
    Springcm,

    [JsonStringEnumMemberName("zendesk")]
    Zendesk,

    [JsonStringEnumMemberName("zoom")]
    Zoom,

    [JsonStringEnumMemberName("sso_integration")]
    SsoIntegration,

    [JsonStringEnumMemberName("oag")]
    Oag

}
