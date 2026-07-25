using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers.Legacy
{

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v1clientfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3Client"/> while the finalizer class was named <c>V1ClientFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V1ClientFinalizer : EntityFinalizerBase<V2alpha3Client>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1ClientFinalizer(IEntityController<V2alpha3Client> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha1clientfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3Client"/> while the finalizer class was named <c>V2alpha1ClientFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha1ClientFinalizer : EntityFinalizerBase<V2alpha3Client>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha1ClientFinalizer(IEntityController<V2alpha3Client> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha3clientfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3Client"/> while the finalizer class was named <c>V2alpha3ClientFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha3ClientFinalizer : EntityFinalizerBase<V2alpha3Client>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha3ClientFinalizer(IEntityController<V2alpha3Client> controller) :
            base(controller)
        {

        }

    }

}
