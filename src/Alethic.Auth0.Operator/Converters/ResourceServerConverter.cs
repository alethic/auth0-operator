using System.Collections.Generic;
using System.Runtime.Versioning;

using Alethic.Auth0.Operator.Converters.Generated;
using Alethic.Auth0.Operator.Models;

using KubeOps.Operator.Web.Webhooks.Conversion;

namespace Alethic.Auth0.Operator.Converters
{

    using V2alpha3ResourceServerEntity = Models.V2alpha3ResourceServer;

    /// <summary>Provides conversions targeting <see cref="V2alpha3ResourceServer"/>.</summary>
    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha3ResourceServerEntity))]
    public class ResourceServerConverter : ConversionWebhook<V2alpha3ResourceServerEntity>
    {

        protected override IEnumerable<IEntityConverter<V2alpha3ResourceServerEntity>> Converters => [
            new V1ToV2alpha3()
        ];

        class V1ToV2alpha3 : IEntityConverter<V1ResourceServer, V2alpha3ResourceServerEntity>
        {

            public V2alpha3ResourceServerEntity Convert(V1ResourceServer from)
            {
                var result = new V2alpha3ResourceServerEntity { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.Init = ResourceServerCopy.Convert(from.Spec.Init);
                result.Spec.Conf = ResourceServerCopy.Convert(from.Spec.Conf);
                result.Status.Id = from.Status.Id;
                result.Status.Identifier = from.Status.Identifier;
                result.Status.LastConf = ResourceServerCopy.Convert(from.Status.LastConf);
                return result;
            }

            public V1ResourceServer Revert(V2alpha3ResourceServerEntity source)
            {
                var result = new V1ResourceServer { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.TenantRef = source.Spec.TenantRef;
                result.Spec.Init = ResourceServerCopy.Revert(source.Spec.Init);
                result.Spec.Conf = ResourceServerCopy.Revert(source.Spec.Conf);
                result.Status.Id = source.Status.Id;
                result.Status.Identifier = source.Status.Identifier;
                result.Status.LastConf = ResourceServerCopy.Revert(source.Status.LastConf);
                return result;
            }

        }

    }

}
