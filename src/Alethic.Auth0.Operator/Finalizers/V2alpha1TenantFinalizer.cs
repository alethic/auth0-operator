using System.Threading;
using System.Threading.Tasks;

using Alethic.Auth0.Operator.Models;

using KubeOps.Abstractions.Finalizer;

namespace Alethic.Auth0.Operator.Finalizers
{

    public class V2alpha1TenantFinalizer : IEntityFinalizer<V2alpha1Tenant>
    {

        public Task FinalizeAsync(V2alpha1Tenant entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

    }

}
