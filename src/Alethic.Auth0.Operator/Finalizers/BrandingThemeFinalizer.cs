using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation.Controller;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// Finalizes a <see cref="V2alpha3BrandingTheme"/>. Writes <c>kubernetes.auth0.com/brandingthemefinalizer</c>; the
    /// name deliberately carries no schema version so that it survives future storage version changes.
    /// </summary>
    public class BrandingThemeFinalizer : EntityFinalizerBase<V2alpha3BrandingTheme>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public BrandingThemeFinalizer(IEntityController<V2alpha3BrandingTheme> controller) :
            base(controller)
        {

        }

    }

}
