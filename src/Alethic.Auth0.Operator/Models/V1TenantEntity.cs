namespace Alethic.Auth0.Operator.Models
{

    public interface V1TenantEntity<TSpec, TStatus, TConf, TLastConf> : V1Entity<TSpec, TStatus, TConf, TLastConf>
        where TSpec : V1TenantEntitySpec<TConf>
        where TStatus : V1TenantEntityStatus<TLastConf>
        where TConf : class
        where TLastConf : class
    {



    }

}
