using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.CustomText.V1alpha1;
using Alethic.Auth0.Operator.Core.Models.CustomText.V2alpha3;

namespace Alethic.Auth0.Operator.Converters.Generated
{

    /// <summary>Generated property-copy converter between V1alpha1 and V2alpha3 CustomText models.</summary>
    internal static class CustomTextCopy
    {

        static TTo MapEnum<TFrom, TTo>(TFrom v) where TFrom : struct, Enum where TTo : struct, Enum
            => Enum.Parse<TTo>(Enum.GetName(v) ?? throw new InvalidOperationException($"Unmappable enum value {v}."));

        static TTo? MapEnumN<TFrom, TTo>(TFrom? v) where TFrom : struct, Enum where TTo : struct, Enum
            => v is { } x ? MapEnum<TFrom, TTo>(x) : null;

        public static V2alpha3CustomTextConf? Convert(V1alpha1CustomTextConf? s)
        {
            if (s is null) return null;
            return new V2alpha3CustomTextConf
            {
                Screens = s.Screens?.ToDictionary(__x => __x.Key, __x => Convert(__x.Value)!),
            };
        }

        public static V1alpha1CustomTextConf? Revert(V2alpha3CustomTextConf? s)
        {
            if (s is null) return null;
            return new V1alpha1CustomTextConf
            {
                Screens = s.Screens?.ToDictionary(__x => __x.Key, __x => Revert(__x.Value)!),
            };
        }

        public static V2alpha3CustomTextScreen? Convert(V1alpha1CustomTextScreen? s)
        {
            if (s is null) return null;
            return new V2alpha3CustomTextScreen
            {
            };
        }

        public static V1alpha1CustomTextScreen? Revert(V2alpha3CustomTextScreen? s)
        {
            if (s is null) return null;
            return new V1alpha1CustomTextScreen
            {
            };
        }

    }

}
