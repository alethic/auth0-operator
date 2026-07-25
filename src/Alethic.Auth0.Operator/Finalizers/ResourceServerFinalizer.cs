using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// Finalizes a <see cref="V2alpha3ResourceServer"/>. Writes <c>kubernetes.auth0.com/resourceserverfinalizer</c>; the
    /// name deliberately carries no schema version so that it survives future storage version changes.
    /// </summary>
    public class ResourceServerFinalizer : EntityFinalizerBase<V2alpha3ResourceServer>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public ResourceServerFinalizer(IEntityController<V2alpha3ResourceServer> controller) :
            base(controller)
        {

        }

    }

}
