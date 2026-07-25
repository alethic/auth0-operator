using System;
using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Finalizers;
using Alethic.Auth0.Operator.Models;

using k8s;
using k8s.Models;

using KubeOps.Abstractions.Reconciliation.Finalizer;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    /// <summary>
    /// Pins the finalizer identifiers written into <c>metadata.finalizers</c>. These strings are load bearing across
    /// releases: an identifier that stops being registered wedges deletion of every entity already carrying it,
    /// because the KubeOps reconciler inspects only the first finalizer and gives up when nothing is registered for it.
    /// </summary>
    [TestClass]
    public class EntityFinalizersTests
    {

        // ──────────────────────── Current identifiers ─────────────────────────────

        /// <summary>
        /// The identifier attached to new entities of each kind. These carry no schema version, so that a future
        /// storage version does not mint yet another identifier.
        /// </summary>
        static readonly Dictionary<Type, string> ExpectedCurrent = new()
        {
            [typeof(V2alpha3BrandingTheme)] = "kubernetes.auth0.com/brandingthemefinalizer",
            [typeof(V2alpha3Client)] = "kubernetes.auth0.com/clientfinalizer",
            [typeof(V2alpha3ClientGrant)] = "kubernetes.auth0.com/clientgrantfinalizer",
            [typeof(V2alpha3Connection)] = "kubernetes.auth0.com/connectionfinalizer",
            [typeof(V2alpha3ConnectionClient)] = "kubernetes.auth0.com/connectionclientfinalizer",
            [typeof(V2alpha3CustomDomain)] = "kubernetes.auth0.com/customdomainfinalizer",
            [typeof(V2alpha3ResourceServer)] = "kubernetes.auth0.com/resourceserverfinalizer",
            [typeof(V2alpha3Role)] = "kubernetes.auth0.com/rolefinalizer",
            [typeof(V2alpha3Tenant)] = "kubernetes.auth0.com/tenantfinalizer",
        };

        /// <summary>
        /// Every identifier a released version of this operator has ever attached, and which must therefore remain
        /// registered so entities carrying it can drain. Entries are never removed from this list.
        /// </summary>
        static readonly Dictionary<Type, string[]> ExpectedRetired = new()
        {
            [typeof(V2alpha3BrandingTheme)] = [
                "kubernetes.auth0.com/v1themefinalizer",
                "kubernetes.auth0.com/v1brandingthemefinalizer",
                "kubernetes.auth0.com/v1alpha1brandingthemefinalizer",
                "kubernetes.auth0.com/v2alpha3brandingthemefinalizer"],
            [typeof(V2alpha3Client)] = [
                "kubernetes.auth0.com/v1clientfinalizer",
                "kubernetes.auth0.com/v2alpha1clientfinalizer",
                "kubernetes.auth0.com/v2alpha3clientfinalizer"],
            [typeof(V2alpha3ClientGrant)] = [
                "kubernetes.auth0.com/v1clientgrantfinalizer",
                "kubernetes.auth0.com/v2alpha3clientgrantfinalizer"],
            [typeof(V2alpha3Connection)] = [
                "kubernetes.auth0.com/v1connectionfinalizer",
                "kubernetes.auth0.com/v2alpha1connectionfinalizer",
                "kubernetes.auth0.com/v2alpha3connectionfinalizer"],
            [typeof(V2alpha3ConnectionClient)] = [
                "kubernetes.auth0.com/v2alpha3connectionclientfinalizer"],
            [typeof(V2alpha3CustomDomain)] = [
                "kubernetes.auth0.com/v1alpha1customdomainfinalizer",
                "kubernetes.auth0.com/v2alpha3customdomainfinalizer"],
            [typeof(V2alpha3ResourceServer)] = [
                "kubernetes.auth0.com/v1resourceserverfinalizer",
                "kubernetes.auth0.com/v2alpha3resourceserverfinalizer"],
            [typeof(V2alpha3Role)] = [
                "kubernetes.auth0.com/v2alpha1rolefinalizer",
                "kubernetes.auth0.com/v2alpha3rolefinalizer"],
            [typeof(V2alpha3Tenant)] = [
                "kubernetes.auth0.com/v1tenantfinalizer",
                "kubernetes.auth0.com/v2alpha1tenantfinalizer",
                "kubernetes.auth0.com/v2alpha3tenantfinalizer"],
        };

        [TestMethod]
        public void Current_MatchesExpectedIdentifiers()
        {
            Assert.AreEqual(ExpectedCurrent[typeof(V2alpha3BrandingTheme)], EntityFinalizers.Current<V2alpha3BrandingTheme>());
            Assert.AreEqual(ExpectedCurrent[typeof(V2alpha3Client)], EntityFinalizers.Current<V2alpha3Client>());
            Assert.AreEqual(ExpectedCurrent[typeof(V2alpha3ClientGrant)], EntityFinalizers.Current<V2alpha3ClientGrant>());
            Assert.AreEqual(ExpectedCurrent[typeof(V2alpha3Connection)], EntityFinalizers.Current<V2alpha3Connection>());
            Assert.AreEqual(ExpectedCurrent[typeof(V2alpha3ConnectionClient)], EntityFinalizers.Current<V2alpha3ConnectionClient>());
            Assert.AreEqual(ExpectedCurrent[typeof(V2alpha3CustomDomain)], EntityFinalizers.Current<V2alpha3CustomDomain>());
            Assert.AreEqual(ExpectedCurrent[typeof(V2alpha3ResourceServer)], EntityFinalizers.Current<V2alpha3ResourceServer>());
            Assert.AreEqual(ExpectedCurrent[typeof(V2alpha3Role)], EntityFinalizers.Current<V2alpha3Role>());
            Assert.AreEqual(ExpectedCurrent[typeof(V2alpha3Tenant)], EntityFinalizers.Current<V2alpha3Tenant>());
        }

        [TestMethod]
        public void Current_CarriesNoSchemaVersion()
        {
            foreach (var (entityType, identifier) in ExpectedCurrent)
            {
                var actual = typeof(EntityFinalizers)
                    .GetMethod(nameof(EntityFinalizers.Current))!
                    .MakeGenericMethod(entityType)
                    .Invoke(null, null) as string;

                Assert.AreEqual(identifier, actual);
                StringAssert.DoesNotMatch(actual!, new System.Text.RegularExpressions.Regex(@"/v\d"));
            }
        }

        [TestMethod]
        public void Current_ReturnsNull_ForKindWithoutFinalizer()
        {
            // CustomText cannot be deleted from Auth0, so it is deliberately not finalized
            Assert.IsNull(EntityFinalizers.Current<V2alpha3CustomText>());
            Assert.AreEqual(0, EntityFinalizers.Retired<V2alpha3CustomText>().Count);
        }

        // ──────────────────────── Retired identifiers ─────────────────────────────

        [TestMethod]
        public void Retired_MatchesExpectedIdentifiers()
        {
            AssertRetired<V2alpha3BrandingTheme>();
            AssertRetired<V2alpha3Client>();
            AssertRetired<V2alpha3ClientGrant>();
            AssertRetired<V2alpha3Connection>();
            AssertRetired<V2alpha3ConnectionClient>();
            AssertRetired<V2alpha3CustomDomain>();
            AssertRetired<V2alpha3ResourceServer>();
            AssertRetired<V2alpha3Role>();
            AssertRetired<V2alpha3Tenant>();
        }

        static void AssertRetired<TEntity>()
            where TEntity : IKubernetesObject<V1ObjectMeta>
        {
            CollectionAssert.AreEquivalent(
                ExpectedRetired[typeof(TEntity)],
                EntityFinalizers.Retired<TEntity>().ToArray(),
                $"retired finalizers for {typeof(TEntity).Name}");
        }

        [TestMethod]
        public void Retired_DoesNotContainCurrent()
        {
            foreach (var (entityType, retired) in ExpectedRetired)
                CollectionAssert.DoesNotContain(retired, ExpectedCurrent[entityType]);
        }

        // ──────────────────────── Registration coverage ───────────────────────────

        /// <summary>
        /// The identifiers KubeOps actually registers, keyed by the entity type they were registered for. Anything
        /// this table knows about must appear here, or the operator would attach an identifier it cannot resolve
        /// during finalization.
        /// </summary>
        static Dictionary<Type, HashSet<string>> RegisteredIdentifiers()
        {
            var registered = new Dictionary<Type, HashSet<string>>();

            foreach (var type in typeof(EntityFinalizers).Assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsGenericTypeDefinition)
                    continue;

                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsGenericType == false || iface.GetGenericTypeDefinition() != typeof(IEntityFinalizer<>))
                        continue;

                    var entityType = iface.GetGenericArguments()[0];
                    if (registered.TryGetValue(entityType, out var identifiers) == false)
                        registered[entityType] = identifiers = new HashSet<string>();

                    identifiers.Add(FinalizerIdentifier(type));
                }
            }

            return registered;
        }

        /// <summary>
        /// Reproduces the identifier KubeOps derives from a finalizer's class name, independently of the production
        /// implementation, so that a change to either side is caught here.
        /// </summary>
        static string FinalizerIdentifier(Type finalizerType)
        {
            var name = finalizerType.Name.ToLowerInvariant();
            if (name.EndsWith("finalizer") == false)
                name += "finalizer";

            var identifier = $"kubernetes.auth0.com/{name}";
            if (identifier.Length > 63)
                identifier = identifier.Substring(0, 63);

            return identifier;
        }

        [TestMethod]
        public void EveryKnownIdentifier_HasARegisteredFinalizer()
        {
            var registered = RegisteredIdentifiers();

            foreach (var (entityType, current) in ExpectedCurrent)
            {
                Assert.IsTrue(registered.TryGetValue(entityType, out var identifiers), $"no finalizer registered for {entityType.Name}");
                CollectionAssert.Contains(identifiers!.ToArray(), current, $"no finalizer class produces {current}");

                foreach (var retired in ExpectedRetired[entityType])
                    CollectionAssert.Contains(identifiers!.ToArray(), retired, $"no finalizer class produces {retired}; entities carrying it can never finish deleting");
            }
        }

        [TestMethod]
        public void EveryRegisteredFinalizer_IsKnown()
        {
            foreach (var (entityType, identifiers) in RegisteredIdentifiers())
            {
                Assert.IsTrue(ExpectedCurrent.ContainsKey(entityType), $"{entityType.Name} has finalizers but no entry in this test");

                foreach (var identifier in identifiers)
                    Assert.IsTrue(
                        identifier == ExpectedCurrent[entityType] || ExpectedRetired[entityType].Contains(identifier),
                        $"{identifier} is registered for {entityType.Name} but is neither the current nor a known retired identifier");
            }
        }

    }

}
