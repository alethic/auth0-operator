using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Indicates whether a user attribute is required, optional, or inactive during signup.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1ConnectionOptionsAttributeStatus
    {

        /// <summary>The attribute must be provided at signup.</summary>
        [JsonStringEnumMemberName("required")]
        Required,

        /// <summary>The attribute may optionally be provided at signup.</summary>
        [JsonStringEnumMemberName("optional")]
        Optional,

        /// <summary>The attribute is disabled and not collected during signup.</summary>
        [JsonStringEnumMemberName("inactive")]
        Inactive

    }

}