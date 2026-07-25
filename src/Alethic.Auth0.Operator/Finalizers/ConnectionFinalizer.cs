using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// Finalizes a <see cref="V2alpha3Connection"/>. Writes <c>kubernetes.auth0.com/connectionfinalizer</c>; the
    /// name deliberately carries no schema version so that it survives future storage version changes.
    /// </summary>
    public class ConnectionFinalizer : EntityFinalizerBase<V2alpha3Connection>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public ConnectionFinalizer(IEntityController<V2alpha3Connection> controller) :
            base(controller)
        {

        }

    }

}
