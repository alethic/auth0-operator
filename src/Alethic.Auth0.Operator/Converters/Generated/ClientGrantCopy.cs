using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.ClientGrant.V1;
using Alethic.Auth0.Operator.Core.Models.ClientGrant.V2alpha3;

namespace Alethic.Auth0.Operator.Converters.Generated
{

    /// <summary>Generated property-copy converter between V1 and V2alpha3 ClientGrant models.</summary>
    internal static class ClientGrantCopy
    {

        static TTo MapEnum<TFrom, TTo>(TFrom v) where TFrom : struct, Enum where TTo : struct, Enum
            => Enum.Parse<TTo>(Enum.GetName(v) ?? throw new InvalidOperationException($"Unmappable enum value {v}."));

        static TTo? MapEnumN<TFrom, TTo>(TFrom? v) where TFrom : struct, Enum where TTo : struct, Enum
            => v is { } x ? MapEnum<TFrom, TTo>(x) : null;

        public static V2alpha3ClientGrantConf? Convert(V1ClientGrantConf? s)
        {
            if (s is null) return null;
            return new V2alpha3ClientGrantConf
            {
                ClientRef = s.ClientRef,
                Audience = s.Audience,
                OrganizationUsage = MapEnumN<V1ClientGrantOrganizationUsage, V2alpha3ClientGrantOrganizationUsage>(s.OrganizationUsage),
                AllowAnyOrganization = s.AllowAnyOrganization,
                Scope = s.Scope,
            };
        }

        public static V1ClientGrantConf? Revert(V2alpha3ClientGrantConf? s)
        {
            if (s is null) return null;
            return new V1ClientGrantConf
            {
                ClientRef = s.ClientRef,
                Audience = s.Audience,
                OrganizationUsage = MapEnumN<V2alpha3ClientGrantOrganizationUsage, V1ClientGrantOrganizationUsage>(s.OrganizationUsage),
                AllowAnyOrganization = s.AllowAnyOrganization,
                Scope = s.Scope,
            };
        }

    }

}
