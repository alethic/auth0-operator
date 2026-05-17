using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V1
{

    /// <summary>
    /// Configuration options for the <c>salesforce-community</c> social connection strategy.
    /// Extends <see cref="V1ConnectionSalesforceOptions"/> with community-specific settings.
    /// </summary>
    public record V1ConnectionSalesforceCommunityOptions : V1ConnectionSalesforceOptions
    {
    }

}
