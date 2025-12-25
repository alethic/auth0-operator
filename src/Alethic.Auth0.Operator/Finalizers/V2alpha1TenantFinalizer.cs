using System;
using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Controller;
using KubeOps.Abstractions.Finalizer;

namespace Alethic.Auth0.Operator.Finalizers
{

    public class V2alpha1TenantFinalizer : IEntityFinalizer<V2alpha1Tenant>
    {

        readonly IEntityController<V2alpha1Tenant> _controller;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public V2alpha1TenantFinalizer(IEntityController<V2alpha1Tenant> controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <inheritdoc />
        public async Task FinalizeAsync(V2alpha1Tenant entity, CancellationToken cancellationToken)
        {
            await _controller.DeletedAsync(entity, cancellationToken);
        }


    }

}
