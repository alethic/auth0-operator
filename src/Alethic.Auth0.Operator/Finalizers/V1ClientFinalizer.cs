using System;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Controller;
using KubeOps.Abstractions.Finalizer;

namespace Alethic.Auth0.Operator.Finalizers
{

    public class V1ClientFinalizer : IEntityFinalizer<V1Client>
    {

        readonly IEntityController<V1Client> _controller;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1ClientFinalizer(IEntityController<V1Client> controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <inheritdoc />
        public async Task FinalizeAsync(V1Client entity, CancellationToken cancellationToken)
        {
            await _controller.DeletedAsync(entity, cancellationToken);
        }

    }

}
