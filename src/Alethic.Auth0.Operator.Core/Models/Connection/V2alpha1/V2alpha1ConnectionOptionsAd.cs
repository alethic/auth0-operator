using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1;

public record V2alpha1ConnectionOptionsAd
{

    [JsonPropertyName("agentIP")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AgentIp { get; set; }

    [JsonPropertyName("agentMode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AgentMode { get; set; }

    [JsonPropertyName("agentVersion")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AgentVersion { get; set; }

    [JsonPropertyName("brute_force_protection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BruteForceProtection { get; set; }

    [JsonPropertyName("certAuth")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? CertAuth { get; set; }

    [JsonPropertyName("certs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Certs { get; set; }

    [JsonPropertyName("disable_cache")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableCache { get; set; }

    [JsonPropertyName("disable_self_service_change_password")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? DisableSelfServiceChangePassword { get; set; }

    [JsonPropertyName("domain_aliases")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? DomainAliases { get; set; }

    [JsonPropertyName("icon_url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? IconUrl { get; set; }

    [JsonPropertyName("ips")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Ips { get; set; }

    [JsonPropertyName("kerberos")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Kerberos { get; set; }

    [JsonPropertyName("set_user_root_attributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha1ConnectionSetUserRootAttributesEnum? SetUserRootAttributes { get; set; }

    [JsonPropertyName("signInEndpoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SignInEndpoint { get; set; }

    [JsonPropertyName("tenant_domain")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TenantDomain { get; set; }

    [JsonPropertyName("thumbprints")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? Thumbprints { get; set; }

    [JsonPropertyName("upstream_params")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, V2alpha1ConnectionUpstreamAdditionalProperties>? UpstreamParams { get; set; }

    [JsonPropertyName("non_persistent_attrs")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? NonPersistentAttrs { get; set; }

}
