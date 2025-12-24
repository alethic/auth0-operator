using System.Linq;

namespace Alethic.Auth0.Operator.Models
{

    public interface V1Entity<TSpec, TStatus, TConf, TLastConf>
        where TSpec : V1EntitySpec<TConf>
        where TStatus : V1EntityStatus<TLastConf>
        where TConf : class
        where TLastConf : class
    {

        /// <summary>
        /// Gets the policy set on the entity.
        /// </summary>
        /// <returns></returns>
        public V1EntityPolicyType[] GetPolicy() => Spec.Policy ?? [
            V1EntityPolicyType.Create,
            V1EntityPolicyType.Update,
        ];

        /// <summary>
        /// Gets whether or not this entity has this policy applied.
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public bool HasPolicy(V1EntityPolicyType policy)
        {
            return GetPolicy().Contains(policy);
        }

        /// <summary>
        /// Gets the specification of the entity.
        /// </summary>
        TSpec Spec { get; }

        /// <summary>
        /// Gets the current status of the entity.
        /// </summary>
        TStatus Status { get; }

    }

}
