﻿namespace Alethic.Auth0.Operator.Models
{

    public interface V1TenantEntity<TSpec, TStatus, TConf> : V1Entity<TSpec, TStatus, TConf>
        where TSpec : V1TenantEntitySpec<TConf>
        where TStatus : V1TenantEntityStatus
        where TConf : class
    {



    }

}
