using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers.Legacy
{

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha3connectionclientfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3ConnectionClient"/> while the finalizer class was named <c>V2alpha3ConnectionClientFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha3ConnectionClientFinalizer : EntityFinalizerBase<V2alpha3ConnectionClient>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha3ConnectionClientFinalizer(IEntityController<V2alpha3ConnectionClient> controller) :
            base(controller)
        {

        }

    }

}
