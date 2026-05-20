using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Controls whether an attribute can be used as a login identifier.
    /// </summary>
    public record V2alpha1ConnectionOptionsAttributeIdentifier
    {

        /// <summary>
        /// When <c>true</c>, this attribute is active as a login identifier.
        /// </summary>
        [JsonPropertyName("active")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Active { get; set; }

    }

}