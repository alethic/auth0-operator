using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Core.Models;
using Alethic.Auth0.Operator.Core.Models.ResourceServer.V1;
using Alethic.Auth0.Operator.Core.Models.ResourceServer.V2alpha3;

namespace Alethic.Auth0.Operator.Converters.Generated
{

    /// <summary>Generated property-copy converter between V1 and V2alpha3 ResourceServer models.</summary>
    internal static class ResourceServerCopy
    {

        static TTo MapEnum<TFrom, TTo>(TFrom v) where TFrom : struct, Enum where TTo : struct, Enum
            => Enum.Parse<TTo>(Enum.GetName(v) ?? throw new InvalidOperationException($"Unmappable enum value {v}."));

        static TTo? MapEnumN<TFrom, TTo>(TFrom? v) where TFrom : struct, Enum where TTo : struct, Enum
            => v is { } x ? MapEnum<TFrom, TTo>(x) : null;

        public static V2alpha3ResourceServerAuthorizationDetail? Convert(V1ResourceServerAuthorizationDetail? s)
        {
            if (s is null) return null;
            return new V2alpha3ResourceServerAuthorizationDetail
            {
                Type = s.Type,
            };
        }

        public static V1ResourceServerAuthorizationDetail? Revert(V2alpha3ResourceServerAuthorizationDetail? s)
        {
            if (s is null) return null;
            return new V1ResourceServerAuthorizationDetail
            {
                Type = s.Type,
            };
        }

        public static V2alpha3ResourceServerAuthorizationPolicy? Convert(V1ResourceServerAuthorizationPolicy? s)
        {
            if (s is null) return null;
            return new V2alpha3ResourceServerAuthorizationPolicy
            {
                PolicyId = s.PolicyId,
            };
        }

        public static V1ResourceServerAuthorizationPolicy? Revert(V2alpha3ResourceServerAuthorizationPolicy? s)
        {
            if (s is null) return null;
            return new V1ResourceServerAuthorizationPolicy
            {
                PolicyId = s.PolicyId,
            };
        }

        public static V2alpha3ResourceServerConf? Convert(V1ResourceServerConf? s)
        {
            if (s is null) return null;
            return new V2alpha3ResourceServerConf
            {
                Id = s.Id,
                Identifier = s.Identifier,
                Name = s.Name,
                Scopes = s.Scopes?.Select(__x => Convert(__x)!).ToArray(),
                SigningAlgorithm = MapEnumN<V1ResourceServerSigningAlgorithm, V2alpha3ResourceServerSigningAlgorithm>(s.SigningAlgorithm),
                SigningSecret = s.SigningSecret,
                TokenLifetime = s.TokenLifetime,
                TokenLifetimeForWeb = s.TokenLifetimeForWeb,
                AllowOfflineAccess = s.AllowOfflineAccess,
                AllowOnlineAccess = s.AllowOnlineAccess,
                AllowOnlineAccessWithEphemeralSessions = s.AllowOnlineAccessWithEphemeralSessions,
                SkipConsentForVerifiableFirstPartyClients = s.SkipConsentForVerifiableFirstPartyClients,
                VerificationLocation = s.VerificationLocation,
                TokenDialect = MapEnumN<V1ResourceServerTokenDialect, V2alpha3ResourceServerTokenDialect>(s.TokenDialect),
                EnforcePolicies = s.EnforcePolicies,
                ConsentPolicy = MapEnumN<V1ResourceServerConsentPolicy, V2alpha3ResourceServerConsentPolicy>(s.ConsentPolicy),
                AuthorizationDetails = s.AuthorizationDetails?.Select(__x => Convert(__x)!).ToArray(),
                AuthorizationPolicy = Convert(s.AuthorizationPolicy),
                SubjectTypeAuthorization = Convert(s.SubjectTypeAuthorization),
                TokenEncryption = Convert(s.TokenEncryption),
                ProofOfPossession = Convert(s.ProofOfPossession),
            };
        }

        public static V1ResourceServerConf? Revert(V2alpha3ResourceServerConf? s)
        {
            if (s is null) return null;
            return new V1ResourceServerConf
            {
                Id = s.Id,
                Identifier = s.Identifier,
                Name = s.Name,
                Scopes = s.Scopes?.Select(__x => Revert(__x)!).ToArray(),
                SigningAlgorithm = MapEnumN<V2alpha3ResourceServerSigningAlgorithm, V1ResourceServerSigningAlgorithm>(s.SigningAlgorithm),
                SigningSecret = s.SigningSecret,
                TokenLifetime = s.TokenLifetime,
                TokenLifetimeForWeb = s.TokenLifetimeForWeb,
                AllowOfflineAccess = s.AllowOfflineAccess,
                AllowOnlineAccess = s.AllowOnlineAccess,
                AllowOnlineAccessWithEphemeralSessions = s.AllowOnlineAccessWithEphemeralSessions,
                SkipConsentForVerifiableFirstPartyClients = s.SkipConsentForVerifiableFirstPartyClients,
                VerificationLocation = s.VerificationLocation,
                TokenDialect = MapEnumN<V2alpha3ResourceServerTokenDialect, V1ResourceServerTokenDialect>(s.TokenDialect),
                EnforcePolicies = s.EnforcePolicies,
                ConsentPolicy = MapEnumN<V2alpha3ResourceServerConsentPolicy, V1ResourceServerConsentPolicy>(s.ConsentPolicy),
                AuthorizationDetails = s.AuthorizationDetails?.Select(__x => Revert(__x)!).ToArray(),
                AuthorizationPolicy = Revert(s.AuthorizationPolicy),
                SubjectTypeAuthorization = Revert(s.SubjectTypeAuthorization),
                TokenEncryption = Revert(s.TokenEncryption),
                ProofOfPossession = Revert(s.ProofOfPossession),
            };
        }

        public static V2alpha3ResourceServerProofOfPossession? Convert(V1ResourceServerProofOfPossession? s)
        {
            if (s is null) return null;
            return new V2alpha3ResourceServerProofOfPossession
            {
                Required = s.Required,
                Mechanism = MapEnumN<V1ResourceServerMechanism, V2alpha3ResourceServerMechanism>(s.Mechanism),
            };
        }

        public static V1ResourceServerProofOfPossession? Revert(V2alpha3ResourceServerProofOfPossession? s)
        {
            if (s is null) return null;
            return new V1ResourceServerProofOfPossession
            {
                Required = s.Required,
                Mechanism = MapEnumN<V2alpha3ResourceServerMechanism, V1ResourceServerMechanism>(s.Mechanism),
            };
        }

        public static V2alpha3ResourceServerScope? Convert(V1ResourceServerScope? s)
        {
            if (s is null) return null;
            return new V2alpha3ResourceServerScope
            {
                Value = s.Value,
                Description = s.Description,
            };
        }

        public static V1ResourceServerScope? Revert(V2alpha3ResourceServerScope? s)
        {
            if (s is null) return null;
            return new V1ResourceServerScope
            {
                Value = s.Value,
                Description = s.Description,
            };
        }

        public static V2alpha3ResourceServerSubjectTypeAuthorization? Convert(V1ResourceServerSubjectTypeAuthorization? s)
        {
            if (s is null) return null;
            return new V2alpha3ResourceServerSubjectTypeAuthorization
            {
                Client = Convert(s.Client),
                User = Convert(s.User),
            };
        }

        public static V1ResourceServerSubjectTypeAuthorization? Revert(V2alpha3ResourceServerSubjectTypeAuthorization? s)
        {
            if (s is null) return null;
            return new V1ResourceServerSubjectTypeAuthorization
            {
                Client = Revert(s.Client),
                User = Revert(s.User),
            };
        }

        public static V2alpha3ResourceServerSubjectTypeAuthorizationClient? Convert(V1ResourceServerSubjectTypeAuthorizationClient? s)
        {
            if (s is null) return null;
            return new V2alpha3ResourceServerSubjectTypeAuthorizationClient
            {
                Policy = MapEnumN<V1ResourceServerSubjectTypeAuthorizationClientPolicy, V2alpha3ResourceServerSubjectTypeAuthorizationClientPolicy>(s.Policy),
            };
        }

        public static V1ResourceServerSubjectTypeAuthorizationClient? Revert(V2alpha3ResourceServerSubjectTypeAuthorizationClient? s)
        {
            if (s is null) return null;
            return new V1ResourceServerSubjectTypeAuthorizationClient
            {
                Policy = MapEnumN<V2alpha3ResourceServerSubjectTypeAuthorizationClientPolicy, V1ResourceServerSubjectTypeAuthorizationClientPolicy>(s.Policy),
            };
        }

        public static V2alpha3ResourceServerSubjectTypeAuthorizationUser? Convert(V1ResourceServerSubjectTypeAuthorizationUser? s)
        {
            if (s is null) return null;
            return new V2alpha3ResourceServerSubjectTypeAuthorizationUser
            {
                Policy = MapEnumN<V1ResourceServerSubjectTypeAuthorizationUserPolicy, V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy>(s.Policy),
            };
        }

        public static V1ResourceServerSubjectTypeAuthorizationUser? Revert(V2alpha3ResourceServerSubjectTypeAuthorizationUser? s)
        {
            if (s is null) return null;
            return new V1ResourceServerSubjectTypeAuthorizationUser
            {
                Policy = MapEnumN<V2alpha3ResourceServerSubjectTypeAuthorizationUserPolicy, V1ResourceServerSubjectTypeAuthorizationUserPolicy>(s.Policy),
            };
        }

        public static V2alpha3ResourceServerTokenEncryption? Convert(V1ResourceServerTokenEncryption? s)
        {
            if (s is null) return null;
            return new V2alpha3ResourceServerTokenEncryption
            {
                Format = MapEnumN<V1ResourceServerTokenFormat, V2alpha3ResourceServerTokenFormat>(s.Format),
                EncryptionKey = Convert(s.EncryptionKey),
            };
        }

        public static V1ResourceServerTokenEncryption? Revert(V2alpha3ResourceServerTokenEncryption? s)
        {
            if (s is null) return null;
            return new V1ResourceServerTokenEncryption
            {
                Format = MapEnumN<V2alpha3ResourceServerTokenFormat, V1ResourceServerTokenFormat>(s.Format),
                EncryptionKey = Revert(s.EncryptionKey),
            };
        }

        public static V2alpha3ResourceServerTokenEncryptionKey? Convert(V1ResourceServerTokenEncryptionKey? s)
        {
            if (s is null) return null;
            return new V2alpha3ResourceServerTokenEncryptionKey
            {
                Name = s.Name,
                Algorithm = s.Algorithm,
                Kid = s.Kid,
                Pem = s.Pem,
            };
        }

        public static V1ResourceServerTokenEncryptionKey? Revert(V2alpha3ResourceServerTokenEncryptionKey? s)
        {
            if (s is null) return null;
            return new V1ResourceServerTokenEncryptionKey
            {
                Name = s.Name,
                Algorithm = s.Algorithm,
                Kid = s.Kid,
                Pem = s.Pem,
            };
        }

    }

}
