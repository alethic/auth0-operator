using System;
using System.Threading;
using System.Threading.Tasks;

using k8s;
using k8s.Models;

using KubeOps.Abstractions.Reconciliation;
using KubeOps.Abstractions.Reconciliation.Controller;
using KubeOps.Abstractions.Reconciliation.Finalizer;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// Base for every finalizer of an entity kind. All of the work of finalizing lives on the controller; the
    /// finalizer type exists only because KubeOps derives the identifier it writes into <c>metadata.finalizers</c>
    /// from the finalizer's class name.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntityFinalizerBase<TEntity> : IEntityFinalizer<TEntity>
        where TEntity : IKubernetesObject<V1ObjectMeta>
    {

        readonly IEntityController<TEntity> _controller;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="controller"></param>
        public EntityFinalizerBase(IEntityController<TEntity> controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <inheritdoc />
        public async Task<ReconciliationResult<TEntity>> FinalizeAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return await _controller.DeletedAsync(entity, cancellationToken);
        }

    }

}
