using System;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Controller;
using KubeOps.Abstractions.Finalizer;

namespace Alethic.Auth0.Operator.Finalizers
{

    public class V1ResourceServerFinalizer : IEntityFinalizer<V1ResourceServer>
    {

        readonly IEntityController<V1ResourceServer> _controller;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1ResourceServerFinalizer(IEntityController<V1ResourceServer> controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <inheritdoc />
        public async Task FinalizeAsync(V1ResourceServer entity, CancellationToken cancellationToken)
        {
            await _controller.DeletedAsync(entity, cancellationToken);
        }

    }

}
