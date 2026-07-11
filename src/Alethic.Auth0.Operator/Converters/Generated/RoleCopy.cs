using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.Role.V2alpha1;
using Alethic.Auth0.Operator.Core.Models.Role.V2alpha3;

namespace Alethic.Auth0.Operator.Converters.Generated
{

    /// <summary>Generated property-copy converter between V2alpha1 and V2alpha3 Role models.</summary>
    internal static class RoleCopy
    {

        static TTo MapEnum<TFrom, TTo>(TFrom v) where TFrom : struct, Enum where TTo : struct, Enum
            => Enum.Parse<TTo>(Enum.GetName(v) ?? throw new InvalidOperationException($"Unmappable enum value {v}."));

        static TTo? MapEnumN<TFrom, TTo>(TFrom? v) where TFrom : struct, Enum where TTo : struct, Enum
            => v is { } x ? MapEnum<TFrom, TTo>(x) : null;

        public static V2alpha3RoleConf? Convert(V2alpha1RoleConf? s)
        {
            if (s is null) return null;
            return new V2alpha3RoleConf
            {
                Name = s.Name,
                Description = s.Description,
                Permissions = s.Permissions?.Select(__x => Convert(__x)!).ToArray(),
            };
        }

        public static V2alpha1RoleConf? Revert(V2alpha3RoleConf? s)
        {
            if (s is null) return null;
            return new V2alpha1RoleConf
            {
                Name = s.Name,
                Description = s.Description,
                Permissions = s.Permissions?.Select(__x => Revert(__x)!).ToArray(),
            };
        }

        public static V2alpha3RolePermission? Convert(V2alpha1RolePermission? s)
        {
            if (s is null) return null;
            return new V2alpha3RolePermission
            {
                ResourceServerRef = s.ResourceServerRef,
                Name = s.Name,
            };
        }

        public static V2alpha1RolePermission? Revert(V2alpha3RolePermission? s)
        {
            if (s is null) return null;
            return new V2alpha1RolePermission
            {
                ResourceServerRef = s.ResourceServerRef,
                Name = s.Name,
            };
        }

    }

}
