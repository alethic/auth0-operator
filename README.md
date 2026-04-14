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

- [x] kubernetes.auth0.com/v1:Tenant `a0tenant`
- [x] kubernetes.auth0.com/v2alpha1:Tenant `a0tenant`
- [x] kubernetes.auth0.com/v1:Client `a0app`
- [x] kubernetes.auth0.com/v1:ClientGrant `a0cgr`
- [x] kubernetes.auth0.com/v1:ResourceServer `a0api`
- [x] kubernetes.auth0.com/v1:Connection `a0con`

## Examples

The following examples demonstrate each resource type. All resources are namespaced.

### Tenant (v1)

The `v1` Tenant schema stores all tenant settings as flat properties directly under `spec.conf`.

```yaml
apiVersion: kubernetes.auth0.com/v1
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
    friendly_name: My Tenant
    session_lifetime: 168
    idle_session_lifetime: 72
```

### Tenant (v2alpha1)

The `v2alpha1` Tenant schema organises configuration into `settings`, `branding`, and `prompts` subsections under `spec.conf`.

```yaml
apiVersion: kubernetes.auth0.com/v2alpha1
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
      friendly_name: My Tenant
      session_lifetime: 168
      idle_session_lifetime: 72
      allowed_logout_urls:
        - "https://yourapp.com/callback"
      enabled_locales:
        - "en"
        - "es"
    branding:
      logo_url: https://example.com/logo.png
      favicon_url: https://example.com/favicon.ico
      colors:
        primary: "#0059d6"
        page_background: "#ffffff"
    prompts:
      universal_login_experience: new
      identifier_first: true
```

### Client (App)

The `Client` resource manages an Auth0 [application](https://auth0.com/docs/get-started/applications). An optional `spec.find` entry can locate an existing client by `client_id` or `name`. An optional `spec.secretRef` can point to a Kubernetes secret to create or reference containing the `client_id` and `client_secret` values.

```yaml
apiVersion: kubernetes.auth0.com/v1
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
    app_type: spa
  conf:
    name: example-client
    app_type: spa
    grant_types:
      - authorization_code
    callbacks:
      - https://example.com/callback
    allowed_logout_urls:
      - https://example.com
    web_origins:
      - https://example.com
```

### ResourceServer (API)

The `ResourceServer` resource manages an Auth0 [API (resource server)](https://auth0.com/docs/get-started/apis).

```yaml
apiVersion: kubernetes.auth0.com/v1
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
    allow_offline_access: false
    skip_consent_for_verifiable_first_party_clients: true
    token_lifetime: 86400
    token_lifetime_for_web: 7200
    signing_alg: RS256
    token_dialect: access_token
    scopes:
      - value: read:data
        description: Read data
```

### ClientGrant

Grants permission for a `Client` to access a `ResourceServer`. See the [Client Grants API reference](https://auth0.com/docs/api/management/v2/client-grants). The `clientRef` and `audience` fields reference other operator-managed resources by name or directly by Auth0 ID/identifier.

```yaml
apiVersion: kubernetes.auth0.com/v1
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

The `Connection` resource manages an Auth0 [connection (identity provider)](https://auth0.com/docs/authenticate/identity-providers). An optional `spec.find` entry can locate an existing connection by its Auth0 `id`. The `enabled_clients` field accepts references to operator-managed `Client` resources by name or directly by Auth0 client ID.

```yaml
apiVersion: kubernetes.auth0.com/v1
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
    display_name: Example Connection
    strategy: auth0
    enabled_clients:
      - name: example-client
    options:
      requires_username: false
      brute_force_protection: true
```

## Reference

This section documents shared fields that appear across multiple resource types.

### `spec.policy`

Controls which operations the operator is allowed to perform on the Auth0 entity:

| Value    | Description                          |
|----------|--------------------------------------|
| `Create` | Allow creating new entities          |
| `Update` | Allow updating existing entities     |
| `Delete` | Allow deleting remote entities       |

### `spec.find`

Available on `Client` and `Connection` resources to locate an existing Auth0 entity instead of creating a new one.

**Client find fields:**

| Field       | Description                                  |
|-------------|----------------------------------------------|
| `client_id` | Match by Auth0 client ID                     |
| `name`      | Match by application name                    |

**Connection find fields:**

| Field | Description                    |
|-------|--------------------------------|
| `id`  | Match by Auth0 connection ID   |

### `spec.secretRef`

The `Client` resource supports an optional `secretRef` field which can point to either an existing secret or the name of a secret to be created containing the `client_id` and `client_secret` values extracted from the app.

### Cross-resource references

Some fields reference other operator-managed Kubernetes resources:

- **`tenantRef`** — `{ name, namespace? }` — references a `Tenant` resource.
- **`clientRef`** — `{ name, namespace?, id? }` — references a `Client` resource by Kubernetes name or Auth0 `client_id`.
- **`audience`** — `{ name, namespace?, id?, identifier? }` — references a `ResourceServer` by Kubernetes name, Auth0 ID, or API `identifier`.
- **`enabled_clients`** — array of `{ name, namespace?, id? }` — references `Client` resources.
