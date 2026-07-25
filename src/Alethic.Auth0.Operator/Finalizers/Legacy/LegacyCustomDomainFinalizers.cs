using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers.Legacy
{

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v1alpha1customdomainfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3CustomDomain"/> while the finalizer class was named <c>V1alpha1CustomDomainFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V1alpha1CustomDomainFinalizer : EntityFinalizerBase<V2alpha3CustomDomain>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1alpha1CustomDomainFinalizer(IEntityController<V2alpha3CustomDomain> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha3customdomainfinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3CustomDomain"/> while the finalizer class was named <c>V2alpha3CustomDomainFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha3CustomDomainFinalizer : EntityFinalizerBase<V2alpha3CustomDomain>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha3CustomDomainFinalizer(IEntityController<V2alpha3CustomDomain> controller) :
            base(controller)
        {

        }

    }

}
