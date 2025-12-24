using System.Collections;

namespace Alethic.Auth0.Operator.Models
{

    public interface ApiEntityStatus<TLastConf>
        where TLastConf: class
    {

        TLastConf? LastConf { get; set; }

    }

}
