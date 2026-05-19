using System;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionOptionsPaypal
{

    [JsonPropertyName("client_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientId { get; set; }

    [JsonPropertyName("client_secret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("freeform_scopes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? FreeformScopes { get; set; }

    [JsonPropertyName("scope")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Scope { get; set; }

    [JsonPropertyName("set_user_root_attributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionSetUserRootAttributesEnum? SetUserRootAttributes { get; set; }

    [JsonPropertyName("address")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Address { get; set; }

    [JsonPropertyName("email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Email { get; set; }

    [JsonPropertyName("phone")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Phone { get; set; }

    [JsonPropertyName("profile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Profile { get; set; }

    [JsonPropertyName("non_persistent_attrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
