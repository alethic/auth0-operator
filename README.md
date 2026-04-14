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
- [x] kubernetes.auth0.com/v1alpha1:BrandingTheme `a0theme`
- [x] kubernetes.auth0.com/v1alpha1:CustomDomain `a0domain`
- [x] kubernetes.auth0.com/v1alpha1:CustomText `a0customtext`

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

### BrandingTheme

The `BrandingTheme` resource manages the [branding theme](https://auth0.com/docs/customize/branding/branding-themes) for a tenant, controlling the visual appearance of login pages and other Auth0-hosted screens. An optional `spec.find` entry can locate an existing theme by its Auth0 `id`.

```yaml
apiVersion: kubernetes.auth0.com/v1alpha1
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
      button_border_radius: 3
      button_border_weight: 1
      buttons_style: rounded
      input_border_radius: 3
      input_border_weight: 1
      inputs_style: rounded
      show_widget_shadow: true
      widget_border_weight: 0
      widget_corner_radius: 5
    colors:
      primary_button: "#0059d6"
      primary_button_label: "#ffffff"
      body_text: "#1e212a"
      header: "#1e212a"
      icons: "#65676e"
      input_background: "#ffffff"
      input_border: "#c9cace"
      input_filled_text: "#1e212a"
      input_labels_placeholders: "#65676e"
      links_focused_components: "#0059d6"
      widget_background: "#ffffff"
      widget_border: "#c9cace"
      error: "#d03c38"
      success: "#13a688"
    fonts:
      font_url: https://fonts.googleapis.com/css2?family=Inter
      reference_text_size: 16
      title:
        bold: true
        size: 150
      subtitle:
        bold: false
        size: 100
      body_text:
        bold: false
        size: 87.5
      buttons_text:
        bold: false
        size: 100
      input_labels:
        bold: false
        size: 100
      links:
        bold: true
        size: 87.5
      links_style: normal
    page_background:
      background_color: "#f0f0f0"
      background_image_url: https://example.com/bg.png
      page_layout: center
    widget:
      header_text_alignment: center
      logo_height: 52
      logo_position: center
      logo_url: https://example.com/logo.png
      social_buttons_layout: bottom
```

### CustomDomain

The `CustomDomain` resource manages Auth0 [custom domains](https://auth0.com/docs/customize/custom-domains). The operator finds existing custom domains by matching on the `domain` field. An optional `spec.secretRef` can reference a Kubernetes secret.

```yaml
apiVersion: kubernetes.auth0.com/v1alpha1
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
    type: auth0_managed_certs
    verification_method: txt
    primary: true
    tls_policy: recommended
    custom_client_ip_header: X-Forwarded-For
```

### CustomText

The `CustomText` resource manages Auth0 custom text [localization](https://auth0.com/docs/customize/ui-features/localization) for a tenant. The operator finds existing custom text entries by matching on the `prompt` and `language` fields.

```yaml
apiVersion: kubernetes.auth0.com/v1alpha1
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
| `client_id`| Match by Auth0 client ID            |
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
- `enabled_clients`: References `Client` resources by name, used in `Connection` resources.
