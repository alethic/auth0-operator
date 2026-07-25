using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers.Legacy
{

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v1resourceserverfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3ResourceServer"/> while the finalizer class was named <c>V1ResourceServerFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V1ResourceServerFinalizer : EntityFinalizerBase<V2alpha3ResourceServer>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1ResourceServerFinalizer(IEntityController<V2alpha3ResourceServer> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha3resourceserverfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3ResourceServer"/> while the finalizer class was named <c>V2alpha3ResourceServerFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha3ResourceServerFinalizer : EntityFinalizerBase<V2alpha3ResourceServer>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha3ResourceServerFinalizer(IEntityController<V2alpha3ResourceServer> controller) :
            base(controller)
        {

        }

    }

}
