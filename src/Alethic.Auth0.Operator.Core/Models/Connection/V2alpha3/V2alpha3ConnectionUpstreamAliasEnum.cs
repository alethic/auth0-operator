using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionUpstreamAliasEnum
{

    [JsonStringEnumMemberName("acrValues")]
    AcrValues,

    [JsonStringEnumMemberName("audience")]
    Audience,

    [JsonStringEnumMemberName("clientId")]
    ClientId,

    [JsonStringEnumMemberName("display")]
    Display,

    [JsonStringEnumMemberName("idTokenHint")]
    IdTokenHint,

    [JsonStringEnumMemberName("loginHint")]
    LoginHint,

    [JsonStringEnumMemberName("maxAge")]
    MaxAge,

    [JsonStringEnumMemberName("prompt")]
    Prompt,

    [JsonStringEnumMemberName("resource")]
    Resource,

    [JsonStringEnumMemberName("responseMode")]
    ResponseMode,

    [JsonStringEnumMemberName("responseType")]
    ResponseType,

    [JsonStringEnumMemberName("uiLocales")]
    UiLocales

}
