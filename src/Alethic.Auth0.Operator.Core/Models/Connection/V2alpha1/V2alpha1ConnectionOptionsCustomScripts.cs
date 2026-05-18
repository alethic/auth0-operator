using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Custom script implementations for CRUD operations executed against a custom database connection.
    /// </summary>
    public record V2alpha1ConnectionOptionsCustomScripts
    {

        /// <summary>
        /// Script that validates user credentials against the custom database.
        /// </summary>
        [JsonPropertyName("login")]
        public string? Login { get; set; }

        /// <summary>
        /// Script that retrieves a user profile from the custom database by email or username.
        /// </summary>
        [JsonPropertyName("get_user")]
        public string? GetUser { get; set; }

        /// <summary>
        /// Script that deletes a user from the custom database.
        /// </summary>
        [JsonPropertyName("delete")]
        public string? Delete { get; set; }

        /// <summary>
        /// Script that changes a user's password in the custom database.
        /// </summary>
        [JsonPropertyName("change_password")]
        public string? ChangePassword { get; set; }

        /// <summary>
        /// Script that marks a user's email as verified in the custom database.
        /// </summary>
        [JsonPropertyName("verify")]
        public string? Verify { get; set; }

        /// <summary>
        /// Script that creates a new user in the custom database.
        /// </summary>
        [JsonPropertyName("create")]
        public string? Create { get; set; }

        /// <summary>
        /// Script that updates a user's username in the custom database.
        /// </summary>
        [JsonPropertyName("change_username")]
        public string? ChangeUsername { get; set; }

        /// <summary>
        /// Script that updates a user's email address in the custom database.
        /// </summary>
        [JsonPropertyName("change_email")]
        public string? ChangeEmail { get; set; }

        /// <summary>
        /// Script that updates a user's phone number in the custom database.
        /// </summary>
        [JsonPropertyName("change_phone_number")]
        public string? ChangePhoneNumber { get; set; }

    }

}