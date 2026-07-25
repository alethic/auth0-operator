using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// Finalizes a <see cref="V2alpha3CustomDomain"/>. Writes <c>kubernetes.auth0.com/customdomainfinalizer</c>; the
    /// name deliberately carries no schema version so that it survives future storage version changes.
    /// </summary>
    public class CustomDomainFinalizer : EntityFinalizerBase<V2alpha3CustomDomain>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public CustomDomainFinalizer(IEntityController<V2alpha3CustomDomain> controller) :
            base(controller)
        {

        }

    }

}
