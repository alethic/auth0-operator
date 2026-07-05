# Kubernetes operator for Auth0 management

## About The Project

This Auth0 Kubernetes Operator is responsible for managing the lifecycle of Auth0 resources in a Kubernetes cluster.

It automates the deployment, configuration, and management of Auth0 resources, such as tenants, clients, connections, resource servers, and client grants.

## Installation

```
helm install -n auth0 auth0 oci://ghcr.io/alethic/auth0-operator
```

## Usage

This operator is a cluster-wide operator. We would like to eventually support namespace-only (TODO).

Each available Auth0 resource type exposed by the management API is mapped nearly 1:1 to a Kubernetes document. Tenant is Tenant, Client is Client, etc. Resources each have a `spec.conf` entry which represents the contents of an Auth0 Management API update or create request to apply. Resources also each have a `spec.init` entry which represents the same schema as `spec.conf`, but is only used on initial resource creation. Additionally, some resources have a `spec.find` entry which determines how the operator locates an existing Auth0 entity.

A secret is required to authenticate with Auth0's management API. This secret must contain the `clientId` and `clientSecret` fields.

At least a single `Tenant` resource is required. This `Tenant` resource must contain `spec.auth` with `domain` and `secretRef` to specify the authentication information.

Other resources, such as `Client`, `ResourceServer`, etc, must have a `spec.tenantRef` value referring to the owning tenant to manage. The name of the Kubernetes resource does not refer to the `name` field in the Auth0 Management API.

Each resource has a `spec.policy` entry which is a list of the following possible values: `Create`, `Update`, `Delete`. These policies determine what permissions the Auth0 operator has: Can it create new entities? Can it update existing entities? Can it delete remote entities?

Since the entire API is derived from the Auth0 Management API their documentation is relevant: [Auth0 Management API](https://auth0.com/docs/api/management/v2).

## Supported Resources

`v2alpha3` is the current (storage) version of every resource. Its schema follows
Kubernetes-standard **camelCase** naming (e.g. `allowedClients`, `appType`, `regularWeb`) as
preparation for the V2 release — a departure from the Auth0 Management API's `snake_case`. The
older versions listed below remain **served but deprecated**; the operator automatically
converts them to `v2alpha3` via conversion webhooks, so existing manifests continue to work.
The older versions will be removed in a future release — migrate to `v2alpha3` when convenient.

| Kind | Current version | Short name | Deprecated versions (served) |
|---|---|---|---|
| Tenant | `v2alpha3` | `a0tenant` | `v2alpha1`, `v1` |
| Client | `v2alpha3` | `a0app` | `v2alpha1`, `v1` |
| Connection | `v2alpha3` | `a0con` | `v2alpha1`, `v1` |
| Role | `v2alpha3` | `a0role` | `v2alpha1` |
| ClientGrant | `v2alpha3` | `a0cgr` | `v1` |
| ResourceServer | `v2alpha3` | `a0api` | `v1` |
| BrandingTheme | `v2alpha3` | `a0theme` | `v1alpha1` |
| CustomDomain | `v2alpha3` | `a0domain` | `v1alpha1` |
| CustomText | `v2alpha3` | `a0customtext` | `v1alpha1` |

> Note: KubeOps requires short names to be unique across a resource's versions, so the short
> names above are defined only on `v2alpha3`. All examples below use `v2alpha3`. Manifests that
> still target an older served version continue to work — the operator converts them
> automatically — but the field names in those older schemas use `snake_case`.

## Naming Conventions

The `v2alpha3` schema favors Kubernetes conventions over the Auth0 Management API's:

- **Field names are `camelCase`.** Where the Auth0 API uses `snake_case` (or `kebab-case`),
  `v2alpha3` uses camelCase — `allowed_clients` → `allowedClients`, `app_type` → `appType`,
  `google-oauth2` → `googleOauth2`.
- **Descriptive enum values are `camelCase` too** — `regular_web` → `regularWeb`,
  `sso_integration` → `ssoIntegration`, `non_rotating` → `nonRotating`,
  `access_token` → `accessToken`.
- **Meaningful external constants keep their canonical Auth0 form.** Standardized
  identifiers are *not* camelCased, because their exact spelling carries meaning:
  - JOSE/JWA algorithms: `HS256`, `RS256`, `RS512`, `PS256`, `ES256`, `Ed25519`, `S256`
  - SAML signature/digest algorithms: `rsa-sha256`, `sha256`; OAuth 1.0: `RSA-SHA1`
  - SAML binding URNs: `urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST`
  - Protocol / API-version identifiers: `azure-active-directory-v1.0`,
    `microsoft-identity-platform-v2.0`, `v2026-1`

You never need to know the Auth0 wire value — the operator translates between the `v2alpha3`
representation and the Auth0 Management API during reconciliation.

## Examples

The following examples demonstrate each resource type. All resources are namespaced.

### Tenant

The Tenant schema organises configuration into `settings`, `branding`, and `prompts` subsections under `spec.conf`.

```yaml
apiVersion: kubernetes.auth0.com/v2alpha3
kind: Tenant
metadata:
  name: example-tenant
  namespace: example
spec:
  name: example-tenant
  auth:
    domain: example-tenant.us.auth0.com
    secretRef:
      name: example-tenant
  policy:
    - Create
    - Update
  conf:
    settings:
      friendlyName: My Tenant
      sessionLifetime: 168
      idleSessionLifetime: 72
      allowedLogoutUrls:
        - "https://yourapp.com/callback"
      enabledLocales:
        - "en"
        - "es"
    branding:
      logoUrl: https://example.com/logo.png
      faviconUrl: https://example.com/favicon.ico
      colors:
        primary: "#0059d6"
        pageBackground: "#ffffff"
    prompts:
      universalLoginExperience: new
      identifierFirst: true
```

### Client (App)

The `Client` resource manages an Auth0 [application](https://auth0.com/docs/get-started/applications). An optional `spec.find` entry can locate an existing client by `clientId` or `name`. An optional `spec.secretRef` can point to a Kubernetes secret to create or reference containing the `clientId` and `clientSecret` values.

```yaml
apiVersion: kubernetes.auth0.com/v2alpha3
kind: Client
metadata:
  name: example-client
  namespace: example
spec:
  tenantRef:
    name: example-tenant
  secretRef:
    name: example-client-secret
  policy:
    - Create
    - Update
  find:
    name: example-client
  init:
    appType: spa
  conf:
    name: example-client
    appType: spa
    grantTypes:
      - authorization_code
    callbacks:
      - https://example.com/callback
    allowedLogoutUrls:
      - https://example.com
    webOrigins:
      - https://example.com
```

### ResourceServer (API)

The `ResourceServer` resource manages an Auth0 [API (resource server)](https://auth0.com/docs/get-started/apis).

```yaml
apiVersion: kubernetes.auth0.com/v2alpha3
kind: ResourceServer
metadata:
  name: example-api
  namespace: example
spec:
  tenantRef:
    name: example-tenant
  policy:
    - Create
    - Update
  conf:
    identifier: https://example.com/
    name: Example API
    allowOfflineAccess: false
    skipConsentForVerifiableFirstPartyClients: true
    tokenLifetime: 86400
    tokenLifetimeForWeb: 7200
    signingAlg: RS256
    tokenDialect: accessToken
    scopes:
      - value: read:data
        description: Read data
```

### ClientGrant

Grants permission for a `Client` to access a `ResourceServer`. See the [Client Grants API reference](https://auth0.com/docs/api/management/v2/client-grants). The `clientRef` and `audience` fields reference other operator-managed resources by name or directly by Auth0 ID/identifier.

```yaml
apiVersion: kubernetes.auth0.com/v2alpha3
kind: ClientGrant
metadata:
  name: example-app-api
  namespace: example
spec:
  tenantRef:
    name: example-tenant
  policy:
    - Create
    - Update
  conf:
    clientRef:
      name: example-client
    audience:
      name: example-api
    scope:
      - read:data
```

### Connection

The `Connection` resource manages an Auth0 [connection (identity provider)](https://auth0.com/docs/authenticate/identity-providers). An optional `spec.find` entry can locate an existing connection by its Auth0 `id`. The `enabledClients` field accepts references to operator-managed `Client` resources by name or directly by Auth0 client ID.

```yaml
apiVersion: kubernetes.auth0.com/v2alpha3
kind: Connection
metadata:
  name: example-connection
  namespace: example
spec:
  tenantRef:
    name: example-tenant
  policy:
    - Create
    - Update
  find:
    id: con_abc123
  conf:
    name: example-connection
    displayName: Example Connection
    strategy: auth0
    enabledClients:
      - name: example-client
    options:
      requiresUsername: false
      bruteForceProtection: true
```

### Role

The `Role` resource manages an Auth0 [role](https://auth0.com/docs/manage-users/access-control/rbac) and its associated permissions. See the [Roles API reference](https://auth0.com/docs/api/management/v2/roles). Each entry under `permissions` associates a permission (scope) defined on a `ResourceServer` with the role; the `resourceServerRef` field references an operator-managed `ResourceServer` by name or directly by Auth0 ID/identifier. When `permissions` is set, the operator reconciles the role's permissions to exactly match the list; omit it to leave permissions unmanaged.

```yaml
apiVersion: kubernetes.auth0.com/v2alpha3
kind: Role
metadata:
  name: example-role
  namespace: example
spec:
  tenantRef:
    name: example-tenant
  policy:
    - Create
    - Update
  conf:
    name: admin
    description: Administrator role
    permissions:
      - resourceServerRef:
          name: example-api
        name: read:data
      - resourceServerRef:
          name: example-api
        name: write:data
```

### BrandingTheme

The `BrandingTheme` resource manages the [branding theme](https://auth0.com/docs/customize/branding/branding-themes) for a tenant, controlling the visual appearance of login pages and other Auth0-hosted screens. An optional `spec.find` entry can locate an existing theme by its Auth0 `id`.

```yaml
apiVersion: kubernetes.auth0.com/v2alpha3
kind: BrandingTheme
metadata:
  name: example-theme
  namespace: example
spec:
  tenantRef:
    name: example-tenant
  policy:
    - Create
    - Update
  find:
    id: btheme_abc123
  conf:
    displayName: Example Theme
    borders:
      buttonBorderRadius: 3
      buttonBorderWeight: 1
      buttonsStyle: rounded
      inputBorderRadius: 3
      inputBorderWeight: 1
      inputsStyle: rounded
      showWidgetShadow: true
      widgetBorderWeight: 0
      widgetCornerRadius: 5
    colors:
      primaryButton: "#0059d6"
      primaryButtonLabel: "#ffffff"
      bodyText: "#1e212a"
      header: "#1e212a"
      icons: "#65676e"
      inputBackground: "#ffffff"
      inputBorder: "#c9cace"
      inputFilledText: "#1e212a"
      inputLabelsPlaceholders: "#65676e"
      linksFocusedComponents: "#0059d6"
      widgetBackground: "#ffffff"
      widgetBorder: "#c9cace"
      error: "#d03c38"
      success: "#13a688"
    fonts:
      fontUrl: https://fonts.googleapis.com/css2?family=Inter
      referenceTextSize: 16
      title:
        bold: true
        size: 150
      subtitle:
        bold: false
        size: 100
      bodyText:
        bold: false
        size: 87.5
      buttonsText:
        bold: false
        size: 100
      inputLabels:
        bold: false
        size: 100
      links:
        bold: true
        size: 87.5
      linksStyle: normal
    pageBackground:
      backgroundColor: "#f0f0f0"
      backgroundImageUrl: https://example.com/bg.png
      pageLayout: center
    widget:
      headerTextAlignment: center
      logoHeight: 52
      logoPosition: center
      logoUrl: https://example.com/logo.png
      socialButtonsLayout: bottom
```

### CustomDomain

The `CustomDomain` resource manages Auth0 [custom domains](https://auth0.com/docs/customize/custom-domains). The operator finds existing custom domains by matching on the `domain` field. An optional `spec.secretRef` can reference a Kubernetes secret.

```yaml
apiVersion: kubernetes.auth0.com/v2alpha3
kind: CustomDomain
metadata:
  name: example-domain
  namespace: example
spec:
  tenantRef:
    name: example-tenant
  policy:
    - Create
    - Update
  conf:
    domain: login.example.com
    type: auth0ManagedCerts
    verificationMethod: txt
    primary: true
    tlsPolicy: recommended
    customClientIpHeader: X-Forwarded-For
```

### CustomText

The `CustomText` resource manages Auth0 custom text [localization](https://auth0.com/docs/customize/ui-features/localization) for a tenant. The operator finds existing custom text entries by matching on the `prompt` and `language` fields.

```yaml
apiVersion: kubernetes.auth0.com/v2alpha3
kind: CustomText
metadata:
  name: example-login-text
  namespace: example
spec:
  tenantRef:
    name: example-tenant
  prompt: login
  language: en
  policy:
    - Update
  conf:
    screens:
      login:
        title: Welcome to Example
        description: Log in to continue
      login-id:
        title: Sign In
        description: Enter your email to get started
```

## Reference

Available on `Client`, `Connection`, and `BrandingTheme` resources to locate an existing Auth0 entity instead of creating a new one.

### spec.find

**Connection find fields:**

| Field | Description                    |
|-------|--------------------------------|
| `id`  | Match by Auth0 connection ID   |

**BrandingTheme find fields:**

| Field | Description                        |
|-------|------------------------------------|
| `id`  | Match by Auth0 branding theme ID   |

**Client find fields:**

| Field     | Description                         |
|-----------|-------------------------------------|
| `clientId`| Match by Auth0 client ID            |
| `name`    | Match by Auth0 client name          |

### spec.policy

| Policy | Description                           |
|--------|---------------------------------------|
| `Create` | Can create new entities                 |
| `Update` | Can update existing entities             |
| `Delete` | Can delete remote entities               |

### spec.secretRef

Used by `Client` and `CustomDomain` resources to refer to a Kubernetes secret containing Auth0 credentials.

### Cross-resource references

- `tenantRef`: References the owning `Tenant` resource.
- `clientRef`: References a `Client` resource, used in `ClientGrant` and `Connection` resources.
- `audience`: References a `ResourceServer` resource, used in `ClientGrant` resources.
- `enabledClients`: References `Client` resources by name, used in `Connection` resources.
- `resourceServerRef`: References a `ResourceServer` resource, used in `Role` permissions.
