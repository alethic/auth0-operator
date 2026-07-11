using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

public record V2alpha3ClientAddons
{

    [JsonPropertyName("aws")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonAws? Aws { get; set; }

    [JsonPropertyName("azureBlob")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonAzureBlob? AzureBlob { get; set; }

    [JsonPropertyName("azureSb")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonAzureSb? AzureSb { get; set; }

    [JsonPropertyName("rms")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonRms? Rms { get; set; }

    [JsonPropertyName("mscrm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonMscrm? Mscrm { get; set; }

    [JsonPropertyName("slack")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonSlack? Slack { get; set; }

    [JsonPropertyName("sentry")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonSentry? Sentry { get; set; }

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
    public V2alpha3ClientAddonEchoSign? Echosign { get; set; }

    [JsonPropertyName("egnyte")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonEgnyte? Egnyte { get; set; }

    [JsonPropertyName("firebase")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonFirebase? Firebase { get; set; }

    [JsonPropertyName("newrelic")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonNewRelic? Newrelic { get; set; }

    [JsonPropertyName("office365")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonOffice365? Office365 { get; set; }

    [JsonPropertyName("salesforce")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonSalesforce? Salesforce { get; set; }

    [JsonPropertyName("salesforceApi")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonSalesforceApi? SalesforceApi { get; set; }

    [JsonPropertyName("salesforceSandboxApi")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonSalesforceSandboxApi? SalesforceSandboxApi { get; set; }

    [JsonPropertyName("samlp")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonSaml? Samlp { get; set; }

    [JsonPropertyName("layer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonLayer? Layer { get; set; }

    [JsonPropertyName("sapApi")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonSapapi? SapApi { get; set; }

    [JsonPropertyName("sharepoint")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonSharePoint? Sharepoint { get; set; }

    [JsonPropertyName("springcm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonSpringCm? Springcm { get; set; }

    [JsonPropertyName("wams")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonWams? Wams { get; set; }

    [JsonPropertyName("wsfed")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object?>? Wsfed { get; set; }

    [JsonPropertyName("zendesk")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonZendesk? Zendesk { get; set; }

    [JsonPropertyName("zoom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonZoom? Zoom { get; set; }

    [JsonPropertyName("ssoIntegration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonSsoIntegration? SsoIntegration { get; set; }

    [JsonPropertyName("oag")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public V2alpha3ClientAddonOag? Oag { get; set; }

}
