using System.Collections.Generic;
using System.Runtime.Versioning;

using Alethic.Auth0.Operator.Converters.Generated;
using Alethic.Auth0.Operator.Models;

using KubeOps.Operator.Web.Webhooks.Conversion;

namespace Alethic.Auth0.Operator.Converters
{

    using V2alpha3CustomTextEntity = Models.V2alpha3CustomText;

    /// <summary>Provides conversions targeting <see cref="V2alpha3CustomText"/>.</summary>
    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha3CustomTextEntity))]
    public class CustomTextConverter : ConversionWebhook<V2alpha3CustomTextEntity>
    {

        protected override IEnumerable<IEntityConverter<V2alpha3CustomTextEntity>> Converters => [
            new V1alpha1ToV2alpha3()
        ];

        class V1alpha1ToV2alpha3 : IEntityConverter<V1alpha1CustomText, V2alpha3CustomTextEntity>
        {

            public V2alpha3CustomTextEntity Convert(V1alpha1CustomText from)
            {
                var result = new V2alpha3CustomTextEntity { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.Prompt = from.Spec.Prompt;
                result.Spec.Language = from.Spec.Language;
                result.Spec.Conf = CustomTextCopy.Convert(from.Spec.Conf);
                result.Status.Prompt = from.Status.Prompt;
                result.Status.Language = from.Status.Language;
                result.Status.LastConf = CustomTextCopy.Convert(from.Status.LastConf);
                return result;
            }

            public V1alpha1CustomText Revert(V2alpha3CustomTextEntity source)
            {
                var result = new V1alpha1CustomText { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.TenantRef = source.Spec.TenantRef;
                result.Spec.Prompt = source.Spec.Prompt;
                result.Spec.Language = source.Spec.Language;
                result.Spec.Conf = CustomTextCopy.Revert(source.Spec.Conf);
                result.Status.Prompt = source.Status.Prompt;
                result.Status.Language = source.Status.Language;
                result.Status.LastConf = CustomTextCopy.Revert(source.Status.LastConf);
                return result;
            }

        }

    }

}
