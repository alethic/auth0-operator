using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V2alpha1ConnectionPasswordDefaultDictionaries
    {

        [JsonStringEnumMemberName("en_10k")]
        En10K,

        [JsonStringEnumMemberName("en_100k")]
        En100K,

    }

}