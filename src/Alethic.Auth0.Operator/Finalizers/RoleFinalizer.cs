using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// Finalizes a <see cref="V2alpha3Role"/>. Writes <c>kubernetes.auth0.com/rolefinalizer</c>; the
    /// name deliberately carries no schema version so that it survives future storage version changes.
    /// </summary>
    public class RoleFinalizer : EntityFinalizerBase<V2alpha3Role>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public RoleFinalizer(IEntityController<V2alpha3Role> controller) :
            base(controller)
        {

        }

    }

}
