using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// Finalizes a <see cref="V2alpha3ConnectionClient"/>. Writes <c>kubernetes.auth0.com/connectionclientfinalizer</c>; the
    /// name deliberately carries no schema version so that it survives future storage version changes.
    /// </summary>
    public class ConnectionClientFinalizer : EntityFinalizerBase<V2alpha3ConnectionClient>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public ConnectionClientFinalizer(IEntityController<V2alpha3ConnectionClient> controller) :
            base(controller)
        {

        }

    }

}
