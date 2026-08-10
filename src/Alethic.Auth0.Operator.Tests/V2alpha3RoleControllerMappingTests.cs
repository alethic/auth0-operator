using System.Collections.Generic;
using System.Linq;

using Alethic.Auth0.Operator.Controllers;
using Alethic.Auth0.Operator.Core.Models.Role.V2alpha3;

using Auth0.ManagementApi;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Alethic.Auth0.Operator.Tests
{

    [TestClass]
    public class V2alpha3RoleControllerMappingTests
    {

        // ──────────────────────── FromApi Role ────────────────────────────────────

        [TestMethod]
        public void FromApi_Role_Null_Returns_Null()
        {
            Assert.IsNull(V2alpha3RoleController.FromApi(null, null));
        }

        [TestMethod]
        public void FromApi_Role_MapsScalarProperties()
        {
            var source = new GetRoleResponseContent
            {
                Id = "rol_123",
                Name = "admin",
                Description = "Administrator role",
            };

            var result = V2alpha3RoleController.FromApi(source, null);

            Assert.IsNotNull(result);
            Assert.AreEqual("admin", result.Name);
            Assert.AreEqual("Administrator role", result.Description);
            Assert.IsNull(result.Permissions);
        }

        [TestMethod]
        public void FromApi_Role_MapsPermissions()
        {
            var source = new GetRoleResponseContent { Name = "admin" };
            var permissions = new[]
            {
                new PermissionsResponsePayload { ResourceServerIdentifier = "https://api.example.com/", PermissionName = "read:users" },
                new PermissionsResponsePayload { ResourceServerIdentifier = "https://api.example.com/", PermissionName = "write:users" },
            };

            var result = V2alpha3RoleController.FromApi(source, permissions);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Permissions);
            Assert.AreEqual(2, result.Permissions.Length);
            Assert.AreEqual("read:users", result.Permissions[0].Name);
            Assert.AreEqual("https://api.example.com/", result.Permissions[0].ResourceServerRef?.Identifier);
            Assert.AreEqual("write:users", result.Permissions[1].Name);
        }

        [TestMethod]
        public void FromApi_Permission_MapsFields()
        {
            var result = V2alpha3RoleController.FromApi(new PermissionsResponsePayload
            {
                ResourceServerIdentifier = "https://api.example.com/",
                PermissionName = "read:users",
            });

            Assert.AreEqual("read:users", result.Name);
            Assert.IsNotNull(result.ResourceServerRef);
            Assert.AreEqual("https://api.example.com/", result.ResourceServerRef.Identifier);
        }

        // ──────────────────────── ApplyToApi Create ──────────────────────────────

        [TestMethod]
        public void ApplyToApi_CreateRequest_MapsFields()
        {
            var conf = new V2alpha3RoleConf { Name = "admin", Description = "Administrator role" };
            var req = new CreateRoleRequestContent { Name = "placeholder" };
            V2alpha3RoleController.ApplyToApi(conf, req);

            Assert.AreEqual("admin", req.Name);
            Assert.AreEqual("Administrator role", req.Description);
        }

        [TestMethod]
        public void ApplyToApi_CreateRequest_NullDescription_LeavesUnset()
        {
            var conf = new V2alpha3RoleConf { Name = "admin", Description = null };
            var req = new CreateRoleRequestContent { Name = "placeholder" };
            V2alpha3RoleController.ApplyToApi(conf, req);

            Assert.AreEqual("admin", req.Name);
            Assert.IsNull(req.Description);
        }

        // ──────────────────────── ApplyToApi Update ──────────────────────────────

        [TestMethod]
        public void ApplyToApi_UpdateRequest_MapsFields()
        {
            var conf = new V2alpha3RoleConf { Name = "admin", Description = "Administrator role" };
            var req = new UpdateRoleRequestContent();
            V2alpha3RoleController.ApplyToApi(conf, req);

            Assert.AreEqual("admin", req.Name);
            Assert.AreEqual("Administrator role", req.Description);
        }

        // ──────────────────────── Roundtrip ──────────────────────────────────────

        [TestMethod]
        public void Roundtrip_Permissions_PreservesValues()
        {
            var source = new GetRoleResponseContent { Name = "admin", Description = "desc" };
            var permissions = new List<PermissionsResponsePayload>
            {
                new PermissionsResponsePayload { ResourceServerIdentifier = "https://api.example.com/", PermissionName = "read:users" },
            };

            var conf = V2alpha3RoleController.FromApi(source, permissions);
            Assert.IsNotNull(conf);

            var createReq = new CreateRoleRequestContent { Name = "placeholder" };
            V2alpha3RoleController.ApplyToApi(conf, createReq);

            Assert.AreEqual("admin", createReq.Name);
            Assert.AreEqual("desc", createReq.Description);
            Assert.AreEqual("read:users", conf.Permissions?.Single().Name);
        }

        // ──────────────────────── Type / OwnerId ─────────────────────────────────

        [TestMethod]
        public void FromApi_Role_MapsTypeAndOwnerId()
        {
            var result = V2alpha3RoleController.FromApi(new GetRoleResponseContent
            {
                Name = "admin",
                Type = new RoleTypeEnum(RoleTypeEnum.Values.Organization),
                OwnerId = "org_123",
            }, null);

            Assert.IsNotNull(result);
            Assert.AreEqual(V2alpha3RoleTypeEnum.Organization, result.Type);
            Assert.AreEqual("org_123", result.OwnerId);
        }

        [TestMethod]
        public void FromApi_Role_NullType_MapsNull()
        {
            var result = V2alpha3RoleController.FromApi(new GetRoleResponseContent { Name = "admin" }, null);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Type);
            Assert.IsNull(result.OwnerId);
        }

        [TestMethod]
        public void ApplyToApi_Create_SetsTypeAndOwnerId()
        {
            var request = new CreateRoleRequestContent { Name = "admin" };
            V2alpha3RoleController.ApplyToApi(new V2alpha3RoleConf
            {
                Name = "admin",
                Type = V2alpha3RoleTypeEnum.Tenant,
                OwnerId = "owner_123",
            }, request);

            Assert.AreEqual(RoleTypeEnum.Values.Tenant, request.Type?.Value);
            Assert.AreEqual("owner_123", request.OwnerId);
        }

        [TestMethod]
        public void ConfRequiresUpdate_IgnoresTypeAndOwnerId()
        {
            // type and owner_id can only be set at creation; they must never force a PATCH
            var conf = new V2alpha3RoleConf { Name = "admin", Type = V2alpha3RoleTypeEnum.Organization, OwnerId = "org_123" };
            var last = new V2alpha3RoleConf { Name = "admin" };

            Assert.IsFalse(V2alpha3RoleController.ConfRequiresUpdate(conf, last));
        }

    }

}
