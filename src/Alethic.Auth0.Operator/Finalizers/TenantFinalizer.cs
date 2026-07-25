using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// Finalizes a <see cref="V2alpha3Tenant"/>. Writes <c>kubernetes.auth0.com/tenantfinalizer</c>; the
    /// name deliberately carries no schema version so that it survives future storage version changes.
    /// </summary>
    public class TenantFinalizer : EntityFinalizerBase<V2alpha3Tenant>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public TenantFinalizer(IEntityController<V2alpha3Tenant> controller) :
            base(controller)
        {

        }

    }

}
