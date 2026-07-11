using System;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.Abstractions.Reconciliation.Finalizer;

namespace Alethic.Auth0.Operator.Finalizers
{

    public class V2alpha3BrandingThemeFinalizer : IEntityFinalizer<V2alpha3BrandingTheme>
    {

        readonly IEntityController<V2alpha3BrandingTheme> _controller;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha3BrandingThemeFinalizer(IEntityController<V2alpha3BrandingTheme> controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <inheritdoc />
        public async Task<ReconciliationResult<V2alpha3BrandingTheme>> FinalizeAsync(V2alpha3BrandingTheme entity, CancellationToken cancellationToken)
        {
            return await _controller.DeletedAsync(entity, cancellationToken);
        }

    }

}
