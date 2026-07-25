using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers.Legacy
{

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v1themefinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3BrandingTheme"/> while the finalizer class was named <c>V1ThemeFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V1ThemeFinalizer : EntityFinalizerBase<V2alpha3BrandingTheme>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1ThemeFinalizer(IEntityController<V2alpha3BrandingTheme> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v1brandingthemefinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3BrandingTheme"/> while the finalizer class was named <c>V1BrandingThemeFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V1BrandingThemeFinalizer : EntityFinalizerBase<V2alpha3BrandingTheme>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1BrandingThemeFinalizer(IEntityController<V2alpha3BrandingTheme> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v1alpha1brandingthemefinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3BrandingTheme"/> while the finalizer class was named <c>V1alpha1BrandingThemeFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V1alpha1BrandingThemeFinalizer : EntityFinalizerBase<V2alpha3BrandingTheme>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1alpha1BrandingThemeFinalizer(IEntityController<V2alpha3BrandingTheme> controller) :
            base(controller)
        {

        }

    }

    /// <summary>
    /// Drains <c>kubernetes.auth0.com/v2alpha3brandingthemefinalizer</c>, the identifier this operator wrote for
    /// <see cref="V2alpha3BrandingTheme"/> while the finalizer class was named <c>V2alpha3BrandingThemeFinalizer</c>. It is never attached to
    /// new entities; it exists so that entities already carrying it can finish deleting.
    /// </summary>
    public class V2alpha3BrandingThemeFinalizer : EntityFinalizerBase<V2alpha3BrandingTheme>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha3BrandingThemeFinalizer(IEntityController<V2alpha3BrandingTheme> controller) :
            base(controller)
        {

        }

    }

}
