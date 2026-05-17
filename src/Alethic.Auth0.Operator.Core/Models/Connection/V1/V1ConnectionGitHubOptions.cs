using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>github</c> social connection strategy.
    /// Each boolean property enables the corresponding GitHub OAuth scope.
    /// </summary>
    public record V1ConnectionGitHubOptions : V1ConnectionSocialOptions
    {

        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Email { get; set; }

        [JsonPropertyName("read_user")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReadUser { get; set; }

        [JsonPropertyName("follow")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Follow { get; set; }

        [JsonPropertyName("public_repo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PublicRepo { get; set; }

        [JsonPropertyName("repo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Repo { get; set; }

        [JsonPropertyName("repo_deployment")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RepoDeployment { get; set; }

        [JsonPropertyName("repo_status")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? RepoStatus { get; set; }

        [JsonPropertyName("delete_repo")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DeleteRepo { get; set; }

        [JsonPropertyName("notifications")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Notifications { get; set; }

        [JsonPropertyName("gist")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Gist { get; set; }

        [JsonPropertyName("read_repo_hook")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReadRepoHook { get; set; }

        [JsonPropertyName("write_repo_hook")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WriteRepoHook { get; set; }

        [JsonPropertyName("admin_repo_hook")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AdminRepoHook { get; set; }

        [JsonPropertyName("read_org")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReadOrg { get; set; }

        [JsonPropertyName("write_org")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WriteOrg { get; set; }

        [JsonPropertyName("admin_org")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AdminOrg { get; set; }

        [JsonPropertyName("read_public_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReadPublicKey { get; set; }

        [JsonPropertyName("write_public_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WritePublicKey { get; set; }

        [JsonPropertyName("admin_public_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AdminPublicKey { get; set; }

        [JsonPropertyName("write_discussion")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WriteDiscussion { get; set; }

        [JsonPropertyName("read_discussion")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReadDiscussion { get; set; }

        [JsonPropertyName("admin_gpg_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AdminGpgKey { get; set; }

        [JsonPropertyName("write_gpg_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WriteGpgKey { get; set; }

        [JsonPropertyName("read_gpg_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ReadGpgKey { get; set; }

        [JsonPropertyName("codespace")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Codespace { get; set; }

        [JsonPropertyName("project")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Project { get; set; }

        [JsonPropertyName("icon_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IconUrl { get; set; }

    }

}
