using System;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Reconciliation;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.Abstractions.Reconciliation.Finalizer;

namespace Alethic.Auth0.Operator.Finalizers
{

    public class V2alpha3ClientFinalizer : IEntityFinalizer<V2alpha3Client>
    {

        readonly IEntityController<V2alpha3Client> _controller;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha3ClientFinalizer(IEntityController<V2alpha3Client> controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <inheritdoc />
        public async Task<ReconciliationResult<V2alpha3Client>> FinalizeAsync(V2alpha3Client entity, CancellationToken cancellationToken)
        {
            return await _controller.DeletedAsync(entity, cancellationToken);
        }

    }

}
