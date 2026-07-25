using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers.Legacy
{

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v1connectionfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3Connection"/> while the finalizer class was named <c>V1ConnectionFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V1ConnectionFinalizer : EntityFinalizerBase<V2alpha3Connection>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1ConnectionFinalizer(IEntityController<V2alpha3Connection> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha1connectionfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3Connection"/> while the finalizer class was named <c>V2alpha1ConnectionFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha1ConnectionFinalizer : EntityFinalizerBase<V2alpha3Connection>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha1ConnectionFinalizer(IEntityController<V2alpha3Connection> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha3connectionfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3Connection"/> while the finalizer class was named <c>V2alpha3ConnectionFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha3ConnectionFinalizer : EntityFinalizerBase<V2alpha3Connection>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha3ConnectionFinalizer(IEntityController<V2alpha3Connection> controller) :
            base(controller)
        {

        }

    }

}
