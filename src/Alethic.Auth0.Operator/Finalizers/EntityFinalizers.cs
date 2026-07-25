using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Alethic.Auth0.Operator.Finalizers.Legacy;
using Alethic.Auth0.Operator.Models;

using k8s;
using k8s.Models;

namespace Alethic.Auth0.Operator.Finalizers
{

    /// <summary>
    /// The single source of truth for the finalizer identifiers this operator writes into
    /// <c>metadata.finalizers</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// KubeOps derives a finalizer's identifier from its class name, both when attaching it
    /// (<c>EntityFinalizerExtensions.GetIdentifierName</c>, which reads <c>GetType().Name</c>) and when resolving it
    /// during finalization (the keyed registration emitted by <c>KubeOps.Generator</c>). Because this repository used
    /// to name finalizers after the storage version, every hub version bump minted a new identifier and left the
    /// previous one behind on live entities.
    /// </para>
    /// <para>
    /// A leftover identifier is not merely untidy. The reconciler examines <c>Finalizers()[0]</c> only, and returns
    /// without removing it when no finalizer is registered under that identifier, so an entity whose first finalizer
    /// belongs to a retired release can never finish deleting. The retired identifiers therefore stay registered — via
    /// the shim classes in <see cref="Legacy"/> — so entities already carrying them drain, and
    /// <see cref="Controllers.ControllerBase{TEntity, TSpec, TStatus, TConf, TLastConf}"/> strips them from live
    /// entities so they are not carried forward. Current finalizer names are version-free and should not change again.
    /// </para>
    /// </remarks>
    public static class EntityFinalizers
    {

        /// <summary>
        /// The API group finalizer identifiers are qualified with. Matches the group of every entity kind.
        /// </summary>
        const string Group = "kubernetes.auth0.com";

        /// <summary>
        /// Maximum length of a finalizer identifier, as enforced by KubeOps.
        /// </summary>
        const int MaxIdentifierLength = 63;

        /// <summary>
        /// The current and retired finalizer identifiers of a single entity kind.
        /// </summary>
        /// <param name="Current"></param>
        /// <param name="Retired"></param>
        record struct Registration(string Current, ImmutableHashSet<string> Retired);

        /// <summary>
        /// The finalizer identifiers of each entity kind. Identifiers are derived from the finalizer types themselves
        /// so that renaming a finalizer class cannot silently desynchronize this table from what KubeOps registers.
        /// </summary>
        static readonly Dictionary<Type, Registration> _registrations = new()
        {
            [typeof(V2alpha3BrandingTheme)] = Register<BrandingThemeFinalizer>(
                Identifier<V1ThemeFinalizer>(),
                Identifier<V1BrandingThemeFinalizer>(),
                Identifier<V1alpha1BrandingThemeFinalizer>(),
                Identifier<V2alpha3BrandingThemeFinalizer>()),
            [typeof(V2alpha3Client)] = Register<ClientFinalizer>(
                Identifier<V1ClientFinalizer>(),
                Identifier<V2alpha1ClientFinalizer>(),
                Identifier<V2alpha3ClientFinalizer>()),
            [typeof(V2alpha3ClientGrant)] = Register<ClientGrantFinalizer>(
                Identifier<V1ClientGrantFinalizer>(),
                Identifier<V2alpha3ClientGrantFinalizer>()),
            [typeof(V2alpha3Connection)] = Register<ConnectionFinalizer>(
                Identifier<V1ConnectionFinalizer>(),
                Identifier<V2alpha1ConnectionFinalizer>(),
                Identifier<V2alpha3ConnectionFinalizer>()),
            [typeof(V2alpha3ConnectionClient)] = Register<ConnectionClientFinalizer>(
                Identifier<V2alpha3ConnectionClientFinalizer>()),
            [typeof(V2alpha3CustomDomain)] = Register<CustomDomainFinalizer>(
                Identifier<V1alpha1CustomDomainFinalizer>(),
                Identifier<V2alpha3CustomDomainFinalizer>()),
            [typeof(V2alpha3ResourceServer)] = Register<ResourceServerFinalizer>(
                Identifier<V1ResourceServerFinalizer>(),
                Identifier<V2alpha3ResourceServerFinalizer>()),
            [typeof(V2alpha3Role)] = Register<RoleFinalizer>(
                Identifier<V2alpha1RoleFinalizer>(),
                Identifier<V2alpha3RoleFinalizer>()),
            [typeof(V2alpha3Tenant)] = Register<TenantFinalizer>(
                Identifier<V1TenantFinalizer>(),
                Identifier<V2alpha1TenantFinalizer>(),
                Identifier<V2alpha3TenantFinalizer>()),
        };

        /// <summary>
        /// Builds the registration for an entity kind whose current finalizer is
        /// <typeparamref name="TFinalizer"/>.
        /// </summary>
        /// <typeparam name="TFinalizer"></typeparam>
        /// <param name="retired"></param>
        /// <returns></returns>
        static Registration Register<TFinalizer>(params string[] retired)
        {
            return new Registration(Identifier<TFinalizer>(), retired.ToImmutableHashSet());
        }

        /// <summary>
        /// Reproduces the identifier KubeOps derives for the given finalizer type. Kept in lockstep with
        /// <c>KubeOps.Generator.Generators.FinalizerRegistrationGenerator.FinalizerName</c>.
        /// </summary>
        /// <typeparam name="TFinalizer"></typeparam>
        /// <returns></returns>
        static string Identifier<TFinalizer>()
        {
            var name = typeof(TFinalizer).Name.ToLowerInvariant();
            if (name.EndsWith("finalizer") == false)
                name += "finalizer";

            var identifier = $"{Group}/{name}";
            if (identifier.Length > MaxIdentifierLength)
                identifier = identifier.Substring(0, MaxIdentifierLength);

            return identifier;
        }

        /// <summary>
        /// Gets the finalizer identifier that should be attached to entities of the given kind, or <c>null</c> when
        /// the kind has no finalizer.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static string? Current<TEntity>()
            where TEntity : IKubernetesObject<V1ObjectMeta>
        {
            return _registrations.TryGetValue(typeof(TEntity), out var registration) ? registration.Current : null;
        }

        /// <summary>
        /// Gets the finalizer identifiers previous releases attached to entities of the given kind, which are still
        /// registered so that entities carrying them can drain, but which are no longer attached.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IReadOnlyCollection<string> Retired<TEntity>()
            where TEntity : IKubernetesObject<V1ObjectMeta>
        {
            return _registrations.TryGetValue(typeof(TEntity), out var registration) ? registration.Retired : ImmutableHashSet<string>.Empty;
        }

    }

}
