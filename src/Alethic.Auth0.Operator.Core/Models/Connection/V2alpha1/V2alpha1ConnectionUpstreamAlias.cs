using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha1ConnectionUpstreamAlias
    {

        [JsonStringEnumMemberName("acr_values")]
        AcrValues,

        [JsonStringEnumMemberName("audience")]
        Audience,

        [JsonStringEnumMemberName("client_id")]
        ClientId,

        [JsonStringEnumMemberName("display")]
        Display,

        [JsonStringEnumMemberName("id_token_hint")]
        IdTokenHint,

        [JsonStringEnumMemberName("login_hint")]
        LoginHint,

        [JsonStringEnumMemberName("max_age")]
        MaxAge,

        [JsonStringEnumMemberName("prompt")]
        Prompt,

        [JsonStringEnumMemberName("resource")]
        Resource,

        [JsonStringEnumMemberName("response_mode")]
        ResponseMode,

        [JsonStringEnumMemberName("response_type")]
        ResponseType,

        [JsonStringEnumMemberName("ui_locales")]
        UiLocales

    }

}
