using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.BrandingTheme.V2alpha3
{

    /// <summary>
    /// Identifier display settings for the login experience. Requires the Auth0 early access
    /// universal_login_theme_identifiers feature to be enabled on the tenant; Auth0 rejects theme writes that
    /// include identifiers when the feature is not enabled.
    /// </summary>
    public record V2alpha3BrandingThemeIdentifiers
    {

        [JsonPropertyName("loginDisplay")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemeLoginDisplay? LoginDisplay { get; set; }

        [JsonPropertyName("otpAutocomplete")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? OtpAutocomplete { get; set; }

        [JsonPropertyName("phoneDisplay")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public V2alpha3BrandingThemePhoneDisplay? PhoneDisplay { get; set; }

    }

}
