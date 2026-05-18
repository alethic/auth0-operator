using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Core.Models.Connection.V2alpha1
{

    /// <summary>
    /// Configuration options for the <c>salesforce-community</c> social connection strategy.
    /// Extends <see cref="V2alpha1ConnectionSalesforceOptions"/> with community-specific settings.
    /// </summary>
    public record V2alpha1ConnectionSalesforceCommunityOptions : V2alpha1ConnectionSalesforceOptions
    {
    }

}
