using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{

    /// <summary>
    /// Custom scripts attached to a social or OAuth connection strategy.
    /// </summary>
    public record V2alpha3ConnectionOptionsScripts
    {

        /// <summary>
        /// JavaScript function that maps the raw identity-provider profile response to an Auth0 user profile.
        /// </summary>
        [JsonPropertyName("fetchUserProfile")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? FetchUserProfile { get; set; }

    }

}
