using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

public record V2alpha3ClientAddonAzureBlob
{

    [JsonPropertyName("accountName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccountName { get; set; }

    [JsonPropertyName("storageAccessKey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StorageAccessKey { get; set; }

    [JsonPropertyName("containerName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContainerName { get; set; }

    [JsonPropertyName("blobName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BlobName { get; set; }

    [JsonPropertyName("expiration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Expiration { get; set; }

    [JsonPropertyName("signedIdentifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SignedIdentifier { get; set; }

    [JsonPropertyName("blobRead")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BlobRead { get; set; }

    [JsonPropertyName("blobWrite")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BlobWrite { get; set; }

    [JsonPropertyName("blobDelete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BlobDelete { get; set; }

    [JsonPropertyName("containerRead")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerRead { get; set; }

    [JsonPropertyName("containerWrite")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerWrite { get; set; }

    [JsonPropertyName("containerDelete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerDelete { get; set; }

    [JsonPropertyName("containerList")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerList { get; set; }

}
