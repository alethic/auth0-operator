using System.Collections.Generic;
using System.Runtime.Versioning;

using Alethic.Auth0.Operator.Converters.Generated;
using Alethic.Auth0.Operator.Models;

using KubeOps.Operator.Web.Webhooks.Conversion;

namespace Alethic.Auth0.Operator.Converters
{

    using V2alpha3RoleEntity = Models.V2alpha3Role;

    /// <summary>Provides conversions targeting <see cref="V2alpha3Role"/>.</summary>
    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha3RoleEntity))]
    public class RoleConverter : ConversionWebhook<V2alpha3RoleEntity>
    {

        protected override IEnumerable<IEntityConverter<V2alpha3RoleEntity>> Converters => [
            new V2alpha1ToV2alpha3()
        ];

        class V2alpha1ToV2alpha3 : IEntityConverter<V2alpha1Role, V2alpha3RoleEntity>
        {

            public V2alpha3RoleEntity Convert(V2alpha1Role from)
            {
                var result = new V2alpha3RoleEntity { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.Init = RoleCopy.Convert(from.Spec.Init);
                result.Spec.Conf = RoleCopy.Convert(from.Spec.Conf);
                result.Status.Id = from.Status.Id;
                result.Status.LastConf = RoleCopy.Convert(from.Status.LastConf);
                return result;
            }

            public V2alpha1Role Revert(V2alpha3RoleEntity source)
            {
                var result = new V2alpha1Role { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.TenantRef = source.Spec.TenantRef;
                result.Spec.Init = RoleCopy.Revert(source.Spec.Init);
                result.Spec.Conf = RoleCopy.Revert(source.Spec.Conf);
                result.Status.Id = source.Status.Id;
                result.Status.LastConf = RoleCopy.Revert(source.Status.LastConf);
                return result;
            }

        }

    }

}
