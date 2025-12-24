namespace Alethic.Auth0.Operator.Models
{

    public interface V1TenantEntityStatus<TLastConf> : ApiEntityStatus<TLastConf>
        where TLastConf : class
    {

        string? Id { get; set; }

    }

}
