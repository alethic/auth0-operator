namespace Alethic.Auth0.Operator.Models
{

    /// <summary>
    /// Status of a tenant entity instance. This is used to store the unique ID of the tenant entity instance in Auth0, as well as the last
    /// configuration applied to it.
    /// </summary>
    /// <typeparam name="TLastConf"></typeparam>
    public interface V1TenantEntityStatus<TLastConf> : ApiEntityStatus<TLastConf>
        where TLastConf : class
    {



    }

}
