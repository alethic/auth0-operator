using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Client.V1;

/// <summary>
/// Azure Blob Storage addon configuration.
/// </summary>
public record V1ClientAddonAzureBlob
{

    /// <summary>
    /// Your Azure storage account name. Usually first segment in your Azure storage URL. e.g. `https://acme-org.blob.core.windows.net` would be the account name `acme-org`.
    /// </summary>
    [JsonPropertyName("accountName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccountName { get; set; }

    /// <summary>
    /// Access key associated with this storage account.
    /// </summary>
    [JsonPropertyName("storageAccessKey")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StorageAccessKey { get; set; }

    /// <summary>
    /// Container to request a token for. e.g. `my-container`.
    /// </summary>
    [JsonPropertyName("containerName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContainerName { get; set; }

    /// <summary>
    /// Entity to request a token for. e.g. `my-blob`. If blank the computed SAS will apply to the entire storage container.
    /// </summary>
    [JsonPropertyName("blobName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BlobName { get; set; }

    /// <summary>
    /// Expiration in minutes for the generated token (default of 5 minutes).
    /// </summary>
    [JsonPropertyName("expiration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Expiration { get; set; }

    /// <summary>
    /// Shared access policy identifier defined in your storage account resource.
    /// </summary>
    [JsonPropertyName("signedIdentifier")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SignedIdentifier { get; set; }

    /// <summary>
    /// Indicates if the issued token has permission to read the content, properties, metadata and block list. Use the blob as the source of a copy operation.
    /// </summary>
    [JsonPropertyName("blob_read")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BlobRead { get; set; }

    /// <summary>
    /// Indicates if the issued token has permission to create or write content, properties, metadata, or block list. Snapshot or lease the blob. Resize the blob (page blob only). Use the blob as the destination of a copy operation within the same account.
    /// </summary>
    [JsonPropertyName("blob_write")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BlobWrite { get; set; }

    /// <summary>
    /// Indicates if the issued token has permission to delete the blob.
    /// </summary>
    [JsonPropertyName("blob_delete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? BlobDelete { get; set; }

    /// <summary>
    /// Indicates if the issued token has permission to read the content, properties, metadata or block list of any blob in the container. Use any blob in the container as the source of a copy operation
    /// </summary>
    [JsonPropertyName("container_read")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerRead { get; set; }

    /// <summary>
    /// Indicates that for any blob in the container if the issued token has permission to create or write content, properties, metadata, or block list. Snapshot or lease the blob. Resize the blob (page blob only). Use the blob as the destination of a copy operation within the same account.
    /// </summary>
    [JsonPropertyName("container_write")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerWrite { get; set; }

    /// <summary>
    /// Indicates if issued token has permission to delete any blob in the container.
    /// </summary>
    [JsonPropertyName("container_delete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerDelete { get; set; }

    /// <summary>
    /// Indicates if the issued token has permission to list blobs in the container.
    /// </summary>
    [JsonPropertyName("container_list")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ContainerList { get; set; }

}
