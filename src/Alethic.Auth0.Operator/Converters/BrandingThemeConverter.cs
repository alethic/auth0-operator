using System.Collections.Generic;
using System.Runtime.Versioning;

using Alethic.Auth0.Operator.Converters.Generated;
using Alethic.Auth0.Operator.Models;

using KubeOps.Operator.Web.Webhooks.Conversion;

namespace Alethic.Auth0.Operator.Converters
{

    using V2alpha3BrandingThemeEntity = Models.V2alpha3BrandingTheme;

    /// <summary>Provides conversions targeting <see cref="V2alpha3BrandingTheme"/>.</summary>
    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha3BrandingThemeEntity))]
    public class BrandingThemeConverter : ConversionWebhook<V2alpha3BrandingThemeEntity>
    {

        protected override IEnumerable<IEntityConverter<V2alpha3BrandingThemeEntity>> Converters => [
            new V1alpha1ToV2alpha3()
        ];

        class V1alpha1ToV2alpha3 : IEntityConverter<V1alpha1BrandingTheme, V2alpha3BrandingThemeEntity>
        {

            public V2alpha3BrandingThemeEntity Convert(V1alpha1BrandingTheme from)
            {
                var result = new V2alpha3BrandingThemeEntity { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.TenantRef = from.Spec.TenantRef;
                result.Spec.Find = BrandingThemeCopy.Convert(from.Spec.Find);
                result.Spec.Init = BrandingThemeCopy.Convert(from.Spec.Init);
                result.Spec.Conf = BrandingThemeCopy.Convert(from.Spec.Conf);
                result.Status.Id = from.Status.Id;
                result.Status.LastConf = BrandingThemeCopy.Convert(from.Status.LastConf);
                return result;
            }

            public V1alpha1BrandingTheme Revert(V2alpha3BrandingThemeEntity source)
            {
                var result = new V1alpha1BrandingTheme { Metadata = source.Metadata };
                result.Spec.Policy = source.Spec.Policy;
                result.Spec.TenantRef = source.Spec.TenantRef;
                result.Spec.Find = BrandingThemeCopy.Revert(source.Spec.Find);
                result.Spec.Init = BrandingThemeCopy.Revert(source.Spec.Init);
                result.Spec.Conf = BrandingThemeCopy.Revert(source.Spec.Conf);
                result.Status.Id = source.Status.Id;
                result.Status.LastConf = BrandingThemeCopy.Revert(source.Status.LastConf);
                return result;
            }

        }

    }

}
