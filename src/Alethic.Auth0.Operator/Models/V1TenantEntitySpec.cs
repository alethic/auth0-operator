using Alethic.Auth0.Operator.Core.Models;

namespace Alethic.Auth0.Operator.Models
{

    public interface V1TenantEntitySpec<TConf> : ApiEntitySpec<TConf>
        where TConf : class
    {

        V1TenantReference? TenantRef { get; set; }

    }

}
