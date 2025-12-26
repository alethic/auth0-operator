using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text.Json;

using Alethic.Auth0.Operator.Core.Models.Tenant;
using Alethic.Auth0.Operator.Models;

using KubeOps.Operator.Web.Webhooks.Conversion;

using Microsoft.IdentityModel.Tokens;

namespace Alethic.Auth0.Operator.Converters
{

    /// <summary>
    /// Provides conversions targeting <see cref="V2alpha1Tenant"/>.
    /// </summary>
    [RequiresPreviewFeatures]
    [ConversionWebhook(typeof(V2alpha1Tenant))]
    public class TenantConverter : ConversionWebhook<V2alpha1Tenant>
    {

        protected override IEnumerable<IEntityConverter<V2alpha1Tenant>> Converters => [
            new V1ToV2alpha1()
        ];

        /// <summary>
        /// Converts from <see cref="V2alpha1Tenant"/> to <see cref="V1Tenant"/>.
        /// </summary>
        class V1ToV2alpha1 : IEntityConverter<V1Tenant, V2alpha1Tenant>
        {

            public V2alpha1Tenant Convert(V1Tenant from)
            {
                var result = new V2alpha1Tenant { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.Name = from.Spec.Name;
                result.Spec.Auth = from.Spec.Auth is { } auth ? new V2alpha1Tenant.SpecDef.AuthDef() { SecretRef = auth.SecretRef, Domain = auth.Domain } : null;
                result.Spec.Init = new V2alpha1TenantConf() { Settings = JsonSerializer.Deserialize<TenantSettings?>(JsonSerializer.Serialize(from.Spec.Init)) };
                result.Spec.Conf = new V2alpha1TenantConf() { Settings = JsonSerializer.Deserialize<TenantSettings?>(JsonSerializer.Serialize(from.Spec.Conf)) };
                result.Status.LastConf = new V2alpha1TenantConf() { Settings = JsonSerializer.Deserialize<TenantSettings?>(JsonSerializer.Serialize(from.Status.LastConf)) };
                return result;
            }

            public V1Tenant Revert(V2alpha1Tenant from)
            {
                var result = new V1Tenant { Metadata = from.Metadata };
                result.Spec.Policy = from.Spec.Policy;
                result.Spec.Name = from.Spec.Name;
                result.Spec.Auth = from.Spec.Auth is { } auth ? new V1Tenant.SpecDef.AuthDef() { SecretRef = auth.SecretRef, Domain = auth.Domain } : null;
                result.Spec.Init = JsonSerializer.Deserialize<V1TenantConf?>(JsonSerializer.Serialize(from.Spec.Init?.Settings));
                result.Spec.Conf = JsonSerializer.Deserialize<V1TenantConf?>(JsonSerializer.Serialize(from.Spec.Conf?.Settings));
                result.Status.LastConf = JsonSerializer.Deserialize<Hashtable?>(JsonSerializer.Serialize(from.Status.LastConf?.Settings));
                return result;
            }

        }

    }

}
