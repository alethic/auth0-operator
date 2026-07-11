using System.Linq;

using Alethic.Auth0.Operator.Converters.Generated;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha1;
using Alethic.Auth0.Operator.Core.Models.Client.V2alpha3;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    /// <summary>
    /// Validates the generated structural (property-copy) converter between the
    /// structurally-identical V2alpha1 and V2alpha3 Client models.
    /// </summary>
    [TestClass]
    public class V2alpha3ClientCopyTests
    {

        static V2alpha1ClientConf SampleV2alpha1() => new()
        {
            Name = "myapp",
            Description = "desc",
            OidcConformant = true,
            AllowedClients = new[] { "a", "b" },
            ApplicationType = V2alpha1ClientAppTypeEnum.RegularWeb,
            TokenEndpointAuthMethod = V2alpha1ClientTokenEndpointAuthMethodEnum.ClientSecretPost,
            Mobile = new V2alpha1ClientMobile { Android = new V2alpha1ClientMobileAndroid() },
        };

        [TestMethod]
        public void Convert_MapsScalarsArraysEnumsAndNestedModels()
        {
            var result = ClientCopy.Convert(SampleV2alpha1());

            Assert.IsNotNull(result);
            Assert.AreEqual("myapp", result.Name);
            Assert.AreEqual("desc", result.Description);
            Assert.AreEqual(true, result.OidcConformant);
            CollectionAssert.AreEqual(new[] { "a", "b" }, result.AllowedClients);
            Assert.AreEqual(V2alpha3ClientAppTypeEnum.RegularWeb, result.ApplicationType);
            Assert.AreEqual(V2alpha3ClientTokenEndpointAuthMethodEnum.ClientSecretPost, result.TokenEndpointAuthMethod);
            Assert.IsNotNull(result.Mobile);
            Assert.IsNotNull(result.Mobile!.Android);
        }

        [TestMethod]
        public void ConvertThenRevert_RoundTripsFaithfully()
        {
            var original = SampleV2alpha1();
            var back = ClientCopy.Revert(ClientCopy.Convert(original));

            Assert.IsNotNull(back);
            Assert.AreEqual(original.Name, back!.Name);
            Assert.AreEqual(original.Description, back.Description);
            Assert.AreEqual(original.OidcConformant, back.OidcConformant);
            CollectionAssert.AreEqual(original.AllowedClients, back.AllowedClients);
            Assert.AreEqual(original.ApplicationType, back.ApplicationType);
            Assert.AreEqual(original.TokenEndpointAuthMethod, back.TokenEndpointAuthMethod);
            Assert.IsNotNull(back.Mobile);
            Assert.IsNotNull(back.Mobile!.Android);
        }

        [TestMethod]
        public void Convert_Null_ReturnsNull()
        {
            Assert.IsNull(ClientCopy.Convert((V2alpha1ClientConf?)null));
            Assert.IsNull(ClientCopy.Revert((V2alpha3ClientConf?)null));
        }

    }

}
