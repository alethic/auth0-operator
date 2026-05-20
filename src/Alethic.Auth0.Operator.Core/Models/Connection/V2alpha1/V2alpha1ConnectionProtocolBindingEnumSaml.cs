using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha1ConnectionProtocolBindingEnumSaml
{

    [JsonStringEnumMemberName("urn_oasis_names_tc_saml20_bindings_http_post")]
    UrnOasisNamesTcSaml20BindingsHttpPost,

    [JsonStringEnumMemberName("urn_oasis_names_tc_saml20_bindings_http_redirect")]
    UrnOasisNamesTcSaml20BindingsHttpRedirect

}
