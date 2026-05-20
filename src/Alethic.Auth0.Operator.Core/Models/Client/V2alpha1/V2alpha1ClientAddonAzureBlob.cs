using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;

public record V2alpha1ClientAddonAzureBlob
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

    [JsonPropertyName("blob_read")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BlobRead { get; set; }

    [JsonPropertyName("blob_write")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BlobWrite { get; set; }

    [JsonPropertyName("blob_delete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BlobDelete { get; set; }

    [JsonPropertyName("container_read")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerRead { get; set; }

    [JsonPropertyName("container_write")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerWrite { get; set; }

    [JsonPropertyName("container_delete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerDelete { get; set; }

    [JsonPropertyName("container_list")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerList { get; set; }

}
