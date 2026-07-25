using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers.Legacy
{

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v1clientgrantfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3ClientGrant"/> while the finalizer class was named <c>V1ClientGrantFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V1ClientGrantFinalizer : EntityFinalizerBase<V2alpha3ClientGrant>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1ClientGrantFinalizer(IEntityController<V2alpha3ClientGrant> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha3clientgrantfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3ClientGrant"/> while the finalizer class was named <c>V2alpha3ClientGrantFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha3ClientGrantFinalizer : EntityFinalizerBase<V2alpha3ClientGrant>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha3ClientGrantFinalizer(IEntityController<V2alpha3ClientGrant> controller) :
            base(controller)
        {

        }

    }

}
