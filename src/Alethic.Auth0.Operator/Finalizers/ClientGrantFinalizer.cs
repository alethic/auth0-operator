using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// Finalizes a <see cref="V2alpha3ClientGrant"/>. Writes <c>kubernetes.auth0.com/clientgrantfinalizer</c>; the
    /// name deliberately carries no schema version so that it survives future storage version changes.
    /// </summary>
    public class ClientGrantFinalizer : EntityFinalizerBase<V2alpha3ClientGrant>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public ClientGrantFinalizer(IEntityController<V2alpha3ClientGrant> controller) :
            base(controller)
        {

        }

    }

}
