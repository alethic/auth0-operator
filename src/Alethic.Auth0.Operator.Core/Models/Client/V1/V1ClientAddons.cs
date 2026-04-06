using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Addons enabled for this client and their associated configurations.
/// </summary>
public record V1ClientAddons
{

    [JsonPropertyName("aws")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonAws? Aws { get; set; }

    [JsonPropertyName("azure_blob")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonAzureBlob? AzureBlob { get; set; }

    [JsonPropertyName("azure_sb")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonAzureSb? AzureSb { get; set; }

    [JsonPropertyName("rms")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonRms? Rms { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("mscrm")]
    public V1ClientAddonMscrm? Mscrm { get; set; }

    [JsonPropertyName("slack")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSlack? Slack { get; set; }

    [JsonPropertyName("sentry")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSentry? Sentry { get; set; }

    [JsonPropertyName("box")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object?>? Box { get; set; }

    [JsonPropertyName("cloudbees")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object?>? Cloudbees { get; set; }

    [JsonPropertyName("concur")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object?>? Concur { get; set; }

    [JsonPropertyName("dropbox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object?>? Dropbox { get; set; }

    [JsonPropertyName("echosign")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonEchoSign? Echosign { get; set; }

    [JsonPropertyName("egnyte")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonEgnyte? Egnyte { get; set; }

    [JsonPropertyName("firebase")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonFirebase? Firebase { get; set; }

    [JsonPropertyName("newrelic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonNewRelic? Newrelic { get; set; }

    [JsonPropertyName("office365")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonOffice365? Office365 { get; set; }

    [JsonPropertyName("salesforce")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSalesforce? Salesforce { get; set; }

    [JsonPropertyName("salesforce_api")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSalesforceApi? SalesforceApi { get; set; }

    [JsonPropertyName("salesforce_sandbox_api")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSalesforceSandboxApi? SalesforceSandboxApi { get; set; }

    [JsonPropertyName("samlp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSaml? Samlp { get; set; }

    [JsonPropertyName("layer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonLayer? Layer { get; set; }

    [JsonPropertyName("sap_api")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSapapi? SapApi { get; set; }

    [JsonPropertyName("sharepoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSharePoint? Sharepoint { get; set; }

    [JsonPropertyName("springcm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSpringCm? Springcm { get; set; }

    [JsonPropertyName("wams")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonWams? Wams { get; set; }

    [JsonPropertyName("wsfed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object?>? Wsfed { get; set; }

    [JsonPropertyName("zendesk")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonZendesk? Zendesk { get; set; }

    [JsonPropertyName("zoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonZoom? Zoom { get; set; }

    [JsonPropertyName("sso_integration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonSsoIntegration? SsoIntegration { get; set; }

    [JsonPropertyName("oag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V1ClientAddonOag? Oag { get; set; }

}
