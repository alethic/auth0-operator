namespace Alethic.Auth0.Operator.Models
{

    public interface V1TenantEntityInstance<TSpec, TStatus, TConf, TLastConf> : V1TenantEntity<TSpec, TStatus, TConf, TLastConf>
        where TSpec : V1TenantEntityInstanceSpec<TConf>
        where TStatus : V1TenantEntityInstanceStatus<TLastConf>
        where TConf : class
        where TLastConf : class
    {



    }

}
