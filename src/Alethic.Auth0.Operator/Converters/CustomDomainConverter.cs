using System.Collections.Generic;
using System.Runtime.Versioning;

using Alethic.Auth0.Operator.Converters.Generated;
using Alethic.Auth0.Operator.Models;

using KubeOps.Operator.Web.Webhooks.Conversion;

namespace Alethic.Auth0.Operator.Converters
{

    using V2alpha3CustomDomainEntity = Models.V2alpha3CustomDomain;

    /// <summary>Provides conversions targeting <see cref="V2alpha3CustomDomain"/>.</summary>
    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha3CustomDomainEntity))]
    public class CustomDomainConverter : ConversionWebhook<V2alpha3CustomDomainEntity>
    {

        protected override IEnumerable<IEntityConverter<V2alpha3CustomDomainEntity>> Converters => [
            new V1alpha1ToV2alpha3()
        ];

        class V1alpha1ToV2alpha3 : IEntityConverter<V1alpha1CustomDomain, V2alpha3CustomDomainEntity>
        {

            public V2alpha3CustomDomainEntity Convert(V1alpha1CustomDomain from)
            {
                var result = new V2alpha3CustomDomainEntity { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.SecretRef = from.Spec.SecretRef;
                result.Spec.Init = CustomDomainCopy.Convert(from.Spec.Init);
                result.Spec.Conf = CustomDomainCopy.Convert(from.Spec.Conf);
                result.Status.Id = from.Status.Id;
                result.Status.LastConf = CustomDomainCopy.Convert(from.Status.LastConf);
                return result;
            }

            public V1alpha1CustomDomain Revert(V2alpha3CustomDomainEntity source)
            {
                var result = new V1alpha1CustomDomain { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.TenantRef = source.Spec.TenantRef;
                result.Spec.SecretRef = source.Spec.SecretRef;
                result.Spec.Init = CustomDomainCopy.Revert(source.Spec.Init);
                result.Spec.Conf = CustomDomainCopy.Revert(source.Spec.Conf);
                result.Status.Id = source.Status.Id;
                result.Status.LastConf = CustomDomainCopy.Revert(source.Status.LastConf);
                return result;
            }

        }

    }

}
