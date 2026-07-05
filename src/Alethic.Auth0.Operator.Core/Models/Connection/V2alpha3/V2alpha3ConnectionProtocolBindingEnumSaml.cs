using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum V2alpha3ConnectionProtocolBindingEnumSaml
{

    [JsonStringEnumMemberName("urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST")]
    UrnOasisNamesTcSaml20BindingsHttpPost,

    [JsonStringEnumMemberName("urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect")]
    UrnOasisNamesTcSaml20BindingsHttpRedirect

}
