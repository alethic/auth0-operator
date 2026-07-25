using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// Finalizes a <see cref="V2alpha3Client"/>. Writes <c>kubernetes.auth0.com/clientfinalizer</c>; the
    /// name deliberately carries no schema version so that it survives future storage version changes.
    /// </summary>
    public class ClientFinalizer : EntityFinalizerBase<V2alpha3Client>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public ClientFinalizer(IEntityController<V2alpha3Client> controller) :
            base(controller)
        {

        }

    }

}
