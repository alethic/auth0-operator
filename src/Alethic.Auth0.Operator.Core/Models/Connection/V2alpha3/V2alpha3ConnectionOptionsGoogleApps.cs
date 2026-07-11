using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3;

public record V2alpha3ConnectionOptionsGoogleApps
{

    [JsonPropertyName("adminAccessToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdminAccessToken { get; set; }

    [JsonPropertyName("adminAccessTokenExpiresin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? AdminAccessTokenExpiresin { get; set; }

    [JsonPropertyName("adminRefreshToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AdminRefreshToken { get; set; }

    [JsonPropertyName("allowSettingLoginScopes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AllowSettingLoginScopes { get; set; }

    [JsonPropertyName("apiEnableGroups")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ApiEnableGroups { get; set; }

    [JsonPropertyName("apiEnableUsers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ApiEnableUsers { get; set; }

    [JsonPropertyName("clientId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientId { get; set; }

    [JsonPropertyName("clientSecret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Domain { get; set; }

    [JsonPropertyName("domainAliases")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? DomainAliases { get; set; }

    [JsonPropertyName("email")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Email { get; set; }

    [JsonPropertyName("extAgreedTerms")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtAgreedTerms { get; set; }

    [JsonPropertyName("extGroups")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtGroups { get; set; }

    [JsonPropertyName("extGroupsExtended")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtGroupsExtended { get; set; }

    [JsonPropertyName("extIsAdmin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtIsAdmin { get; set; }

    [JsonPropertyName("extIsSuspended")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ExtIsSuspended { get; set; }

    [JsonPropertyName("federatedConnectionsAccessTokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionFederatedConnectionsAccessTokens? FederatedConnectionsAccessTokens { get; set; }

    [JsonPropertyName("handleLoginFromSocial")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? HandleLoginFromSocial { get; set; }

    [JsonPropertyName("iconUrl")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconUrl { get; set; }

    [JsonPropertyName("mapUserIdToId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? MapUserIdToId { get; set; }

    [JsonPropertyName("profile")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Profile { get; set; }

    [JsonPropertyName("scope")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Scope { get; set; }

    [JsonPropertyName("setUserRootAttributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ConnectionSetUserRootAttributesEnum? SetUserRootAttributes { get; set; }

    [JsonPropertyName("tenantDomain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TenantDomain { get; set; }

    [JsonPropertyName("upstreamParams")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, V2alpha3ConnectionUpstreamAdditionalProperties>? UpstreamParams { get; set; }

    [JsonPropertyName("nonPersistentAttrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
