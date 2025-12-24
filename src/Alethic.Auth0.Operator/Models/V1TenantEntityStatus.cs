namespace Alethic.Auth0.Operator.Models
{

    public interface V1TenantEntityStatus<TLastConf> : V1EntityStatus<TLastConf>
        where TLastConf : class
    {

        string? Id { get; set; }

    }

}
