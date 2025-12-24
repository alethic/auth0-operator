using System.Linq;

namespace Alethic.Auth0.Operator.Models
{

    /// <summary>
    /// An "API Entity" is a kubernetes entity based off of an Auth0 API entity, having an identifier that maps to an API ID.
    /// </summary>
    /// <typeparam name="TSpec"></typeparam>
    /// <typeparam name="TStatus"></typeparam>
    /// <typeparam name="TConf"></typeparam>
    /// <typeparam name="TLastConf"></typeparam>
    public interface ApiEntity<TSpec, TStatus, TConf, TLastConf>
        where TSpec : ApiEntitySpec<TConf>
        where TStatus : ApiEntityStatus<TLastConf>
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
