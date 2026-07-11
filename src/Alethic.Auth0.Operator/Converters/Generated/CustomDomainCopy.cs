using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.CustomDomain.V1alpha1;
using Alethic.Auth0.Operator.Core.Models.CustomDomain.V2alpha3;

namespace Alethic.Auth0.Operator.Converters.Generated
{

    /// <summary>Generated property-copy converter between V1alpha1 and V2alpha3 CustomDomain models.</summary>
    internal static class CustomDomainCopy
    {

        static TTo MapEnum<TFrom, TTo>(TFrom v) where TFrom : struct, Enum where TTo : struct, Enum
            => Enum.Parse<TTo>(Enum.GetName(v) ?? throw new InvalidOperationException($"Unmappable enum value {v}."));

        static TTo? MapEnumN<TFrom, TTo>(TFrom? v) where TFrom : struct, Enum where TTo : struct, Enum
            => v is { } x ? MapEnum<TFrom, TTo>(x) : null;

        public static V2alpha3CustomDomainConf? Convert(V1alpha1CustomDomainConf? s)
        {
            if (s is null) return null;
            return new V2alpha3CustomDomainConf
            {
                Domain = s.Domain,
                Type = MapEnumN<V1alpha1CustomDomainCertificateProvisioning, V2alpha3CustomDomainCertificateProvisioning>(s.Type),
                VerificationMethod = MapEnumN<V1alpha1CustomDomainVerificationMethod, V2alpha3CustomDomainVerificationMethod>(s.VerificationMethod),
                Primary = s.Primary,
                TlsPolicy = s.TlsPolicy,
                CustomClientIpHeader = s.CustomClientIpHeader,
            };
        }

        public static V1alpha1CustomDomainConf? Revert(V2alpha3CustomDomainConf? s)
        {
            if (s is null) return null;
            return new V1alpha1CustomDomainConf
            {
                Domain = s.Domain,
                Type = MapEnumN<V2alpha3CustomDomainCertificateProvisioning, V1alpha1CustomDomainCertificateProvisioning>(s.Type),
                VerificationMethod = MapEnumN<V2alpha3CustomDomainVerificationMethod, V1alpha1CustomDomainVerificationMethod>(s.VerificationMethod),
                Primary = s.Primary,
                TlsPolicy = s.TlsPolicy,
                CustomClientIpHeader = s.CustomClientIpHeader,
            };
        }

    }

}
