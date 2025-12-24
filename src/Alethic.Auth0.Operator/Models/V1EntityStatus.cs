using System.Collections;

namespace Alethic.Auth0.Operator.Models
{

    public interface V1EntityStatus<TLastConf>
        where TLastConf: class
    {

        TLastConf? LastConf { get; set; }

    }

}
