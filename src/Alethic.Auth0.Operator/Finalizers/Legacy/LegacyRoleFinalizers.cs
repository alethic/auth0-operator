using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers.Legacy
{

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha1rolefinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3Role"/> while the finalizer class was named <c>V2alpha1RoleFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha1RoleFinalizer : EntityFinalizerBase<V2alpha3Role>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha1RoleFinalizer(IEntityController<V2alpha3Role> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha3rolefinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3Role"/> while the finalizer class was named <c>V2alpha3RoleFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha3RoleFinalizer : EntityFinalizerBase<V2alpha3Role>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha3RoleFinalizer(IEntityController<V2alpha3Role> controller) :
            base(controller)
        {

        }

    }

}
