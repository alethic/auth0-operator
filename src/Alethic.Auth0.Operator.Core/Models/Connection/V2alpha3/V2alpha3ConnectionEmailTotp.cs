using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha3
{
    /// <summary>
    /// TOTP (time-based one-time password) configuration for the email connection.
    /// </summary>
    public record V2alpha3ConnectionEmailTotp
    {

        /// <summary>
        /// Length of the one-time password.
        /// </summary>
        [JsonPropertyName("length")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Length { get; set; }

        /// <summary>
        /// Time step in seconds for TOTP code generation.
        /// </summary>
        [JsonPropertyName("timeStep")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TimeStep { get; set; }

    }

}
