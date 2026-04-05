using Alethic.Auth0.Operator.Core.Models;

namespace Alethic.Auth0.Operator.Models
{

    public interface V1TenantEntityInstanceSpec<TConf> : V1TenantEntitySpec<TConf>
        where TConf : class
    {

        /// <summary>
        /// Version of configuration used for initial creation.
        /// </summary>
        TConf? Init { get; set; }

    }

}
