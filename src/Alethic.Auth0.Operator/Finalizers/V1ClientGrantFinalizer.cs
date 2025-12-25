using System;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Controller;
using KubeOps.Abstractions.Finalizer;

namespace Alethic.Auth0.Operator.Finalizers
{

    public class V1ClientGrantFinalizer : IEntityFinalizer<V1ClientGrant>
    {

        readonly IEntityController<V1ClientGrant> _controller;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1ClientGrantFinalizer(IEntityController<V1ClientGrant> controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <inheritdoc />
        public async Task FinalizeAsync(V1ClientGrant entity, CancellationToken cancellationToken)
        {
            await _controller.DeletedAsync(entity, cancellationToken);
        }

    }

}
