using System.Collections.Generic;
using System.Runtime.Versioning;

using Alethic.Auth0.Operator.Converters.Generated;
using Alethic.Auth0.Operator.Models;

using KubeOps.Operator.Web.Webhooks.Conversion;

namespace Alethic.Auth0.Operator.Converters
{

    using V2alpha3ClientGrantEntity = Models.V2alpha3ClientGrant;

    /// <summary>Provides conversions targeting <see cref="V2alpha3ClientGrant"/>.</summary>
    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha3ClientGrantEntity))]
    public class ClientGrantConverter : ConversionWebhook<V2alpha3ClientGrantEntity>
    {

        protected override IEnumerable<IEntityConverter<V2alpha3ClientGrantEntity>> Converters => [
            new V1ToV2alpha3()
        ];

        class V1ToV2alpha3 : IEntityConverter<V1ClientGrant, V2alpha3ClientGrantEntity>
        {

            public V2alpha3ClientGrantEntity Convert(V1ClientGrant from)
            {
                var result = new V2alpha3ClientGrantEntity { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.Init = ClientGrantCopy.Convert(from.Spec.Init);
                result.Spec.Conf = ClientGrantCopy.Convert(from.Spec.Conf);
                result.Status.Id = from.Status.Id;
                result.Status.LastConf = ClientGrantCopy.Convert(from.Status.LastConf);
                return result;
            }

            public V1ClientGrant Revert(V2alpha3ClientGrantEntity source)
            {
                var result = new V1ClientGrant { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.TenantRef = source.Spec.TenantRef;
                result.Spec.Init = ClientGrantCopy.Revert(source.Spec.Init);
                result.Spec.Conf = ClientGrantCopy.Revert(source.Spec.Conf);
                result.Status.Id = source.Status.Id;
                result.Status.LastConf = ClientGrantCopy.Revert(source.Status.LastConf);
                return result;
            }

        }

    }

}
