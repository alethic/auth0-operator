﻿using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Models
{

    public class Mobile
    {

        public class MobileAndroid
        {

            [JsonPropertyName("app_package_name")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? AppPackageName { get; set; }

            [JsonPropertyName("keystore_hash")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? KeystoreHash { get; set; }

        }

        public class MobileIos
        {

            [JsonPropertyName("app_bundle_identifier")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? AppBundleIdentifier { get; set; }

            [JsonPropertyName("team_id")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
            public string? TeamId { get; set; }

        }

        [JsonPropertyName("android")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MobileAndroid? Android { get; set; }

        [JsonPropertyName("ios")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MobileIos? Ios { get; set; }

    }

}
