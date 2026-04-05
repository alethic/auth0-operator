using System.Collections;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1
{

    public class V1ClientAddons
    {

        [JsonPropertyName("aws")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? AmazonWebServices { get; set; }

        [JsonPropertyName("wams")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? AzureMobileServices { get; set; }

        [JsonPropertyName("azure_sb")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? AzureServiceBus { get; set; }

        [JsonPropertyName("box")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? Box { get; set; }

        [JsonPropertyName("cloudbees")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? CloudBees { get; set; }

        [JsonPropertyName("concur")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? Concur { get; set; }

        [JsonPropertyName("dropbox")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? DropBox { get; set; }

        [JsonPropertyName("echosign")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? EchoSign { get; set; }

        [JsonPropertyName("egnyte")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? Egnyte { get; set; }

        [JsonPropertyName("firebase")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? FireBase { get; set; }

        [JsonPropertyName("newrelic")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? NewRelic { get; set; }

        [JsonPropertyName("office365")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? Office365 { get; set; }

        [JsonPropertyName("salesforce")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? SalesForce { get; set; }

        [JsonPropertyName("salesforce_api")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? SalesForceApi { get; set; }

        [JsonPropertyName("salesforce_sandbox_api")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? SalesForceSandboxApi { get; set; }

        [JsonPropertyName("samlp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? SamlP { get; set; }

        [JsonPropertyName("sap_api")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? SapApi { get; set; }

        [JsonPropertyName("sharepoint")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? SharePoint { get; set; }

        [JsonPropertyName("springcm")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? SpringCM { get; set; }

        [JsonPropertyName("webapi")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? WebApi { get; set; }

        [JsonPropertyName("wsfed")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? WsFed { get; set; }

        [JsonPropertyName("zendesk")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? Zendesk { get; set; }

        [JsonPropertyName("zoom")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary? Zoom { get; set; }

    }

}
