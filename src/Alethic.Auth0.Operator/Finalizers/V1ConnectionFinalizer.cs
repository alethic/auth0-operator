using System;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Controller;
using KubeOps.Abstractions.Finalizer;

namespace Alethic.Auth0.Operator.Finalizers
{

    public class V1ConnectionFinalizer : IEntityFinalizer<V1Connection>
    {

        readonly IEntityController<V1Connection> _controller;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V1ConnectionFinalizer(IEntityController<V1Connection> controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <inheritdoc />
        public async Task FinalizeAsync(V1Connection entity, CancellationToken cancellationToken)
        {
            await _controller.DeletedAsync(entity, cancellationToken);
        }

    }

}
