using System;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.Abstractions.Reconciliation.Finalizer;

namespace Alethic.Auth0.Operator.Finalizers
{

    public class V1alpha1BrandingThemeFinalizer : IEntityFinalizer<V1alpha1BrandingTheme>
    {

        readonly IEntityController<V1alpha1BrandingTheme> _controller;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1alpha1BrandingThemeFinalizer(IEntityController<V1alpha1BrandingTheme> controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <inheritdoc />
        public async Task<ReconciliationResult<V1alpha1BrandingTheme>> FinalizeAsync(V1alpha1BrandingTheme entity, CancellationToken cancellationToken)
        {
            return await _controller.DeletedAsync(entity, cancellationToken);
        }

    }

}
