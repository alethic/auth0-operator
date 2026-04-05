namespace Alethic.Auth0.Operator.Models
{

    /// <summary>
    /// Status of a tenant entity instance. This is used to store the unique ID of the tenant entity instance in Auth0, as well as the last
    /// configuration applied to it.
    /// </summary>
    /// <typeparam name="TLastConf"></typeparam>
    public interface V1TenantEntityInstanceStatus<TLastConf> : V1TenantEntityStatus<TLastConf>
        where TLastConf : class
    {

        /// <summary>
        /// Unique ID of the tenant entity instance in Auth0. This is used to correlate the tenant entity instance in Kubernetes with the
        /// corresponding tenant entity instance in Auth0.
        /// </summary>
        string? Id { get; set; }

    }

}
