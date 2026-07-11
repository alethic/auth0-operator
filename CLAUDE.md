# CLAUDE.md

Guidance for working in this repository. Also read `.github/copilot-instructions.md` — its
rules (manual Auth0 mapping, two-line `if` statements, etc.) apply here too.

## What this is

A Kubernetes operator (KubeOps 10.x, .NET 10) that manages Auth0 Management API resources —
Tenant, Client, Connection, ResourceServer, ClientGrant, Role, BrandingTheme, CustomDomain,
CustomText — as namespaced CRDs under group `kubernetes.auth0.com`.

## Layout

- `src/Alethic.Auth0.Operator.Core/Models/<Kind>/<Version>/` — the CRD-facing model types
  (the `conf`/`options` trees). Pure POCOs with `[JsonPropertyName]` / `[JsonStringEnumMemberName]`.
- `src/Alethic.Auth0.Operator/Models/V<ver><Kind>.cs` — the KubernetesEntity classes (Spec/Status).
- `src/Alethic.Auth0.Operator/Controllers/` — one reconciler per **storage** version, plus the
  shared `ControllerBase`, `V1TenantEntityController`, `V1TenantEntityInstanceController` bases.
- `src/Alethic.Auth0.Operator/Converters/` — conversion webhooks (one per Kind).
  `Converters/Generated/<Kind>Copy.cs` — structural property-copy converters (see below).
- `src/Alethic.Auth0.Operator/Finalizers/` — one finalizer per storage version.
- `src/Alethic.Auth0.Operator.Tests/` — MSTest, run via the **Microsoft.Testing.Platform**
  executable (NOT `dotnet test` — that fails on .NET 10). See Build & Test.

## API versioning model

Each Kind has multiple **served** CRD versions; exactly one is the **storage/hub** version.
Currently the hub is **`v2alpha3`** for every Kind; older versions (`v1`, `v1alpha1`,
`v2alpha1`) are served-but-deprecated. Key facts:

- The **hub version = the `ConversionWebhook<T>` target type**. KubeOps marks it as the
  CRD storage version. There is no `[StorageVersion]` attribute in this repo.
- **Only the storage version has a controller + finalizer.** The API server stores everything
  as the hub version and converts other served versions via the operator's conversion webhook;
  the operator watches only the hub. So old versions keep their entity class (for the served
  CRD version + as a conversion source) but have **no** controller/finalizer.
- **Short names must be unique across a Kind's versions** (KubeOps requirement). Define
  `[KubernetesEntityShortNames(...)]` only on the storage version; strip it from older ones.
- CRDs are **generated** by `KubeOps.Generator` at build into
  `src/Alethic.Auth0.Operator/config/*.yaml` and consumed by the Helm chart
  (`charts/auth0-operator`). They are gitignored — never hand-edit them.
- **RBAC is generated but NOT auto-consumed.** The build also emits the operator ClusterRole to
  `src/Alethic.Auth0.Operator/config/operator-role.yaml` (gitignored) from the `[EntityRbac]`
  attributes. The chart's `charts/auth0-operator/templates/clusterrole.yaml` is committed and is
  the source of truth, so it must be **manually synced** whenever you add/remove a Kind or change
  an `[EntityRbac]`. Sync by copying the generated file's **entire `rules:` section verbatim**
  over the chart's `rules:` section (keep the chart's templated `metadata:` header — name/labels/
  annotations — unchanged).
- **The generated `config/` files (CRDs + role) only regenerate reliably in a Release build.** The
  KubeOps generator target runs only when the operator assembly actually recompiled *or*
  `Configuration == Release`; an up-to-date/incremental **Debug** build skips generation, so the
  `config/*.yaml` can be stale. Before syncing the chart (or trusting the transpiled CRDs), run
  `dotnet build -c Release src/Alethic.Auth0.Operator` (or clean-build) to force regeneration.

## How conversion works (`Converters/<Kind>Converter.cs`)

`[ConversionWebhook(typeof(V2alpha3<Kind>))]` with a base of `ConversionWebhook<V2alpha3<Kind>>`
and one `IEntityConverter<TSource, V2alpha3<Kind>>` per older served version. Two flavors:

1. **Structural copy (`<Kind>Copy`)** — used for the *immediate predecessor* ↔ hub, because
   those trees are structurally identical (the hub was cloned from the predecessor; only JSON
   names differ). `Convert`/`Revert` construct objects and set every property; enums map by
   member name via `MapEnum`/`MapEnumN`. **No serialization** — this is deliberate (see the
   copilot rule about not using JSON-based mapping).
2. **Reshaping converters** — Client/Connection/Tenant also convert from `v1`, where the shape
   genuinely differs (e.g. Tenant `v1` flat conf vs `v2alpha3` `settings`/`branding`). That
   logic is hand-written and reuses the storage-version controller's `FromApi`/`ToApi` helpers.

Converters are auto-registered by `RegisterComponents()` in `Program.cs` — no manual wiring.

## How controllers map to Auth0

Controllers translate between the CRD model and the **Auth0 SDK** types by hand, field by field,
in `FromApi`/`ToApi`/`ApplyToApi` helpers. **Never** use `JsonConvertTo` or JSON round-tripping
for Auth0↔model conversion — source/target shapes are incompatible. Extract nested conversions
into their own `FromApi`/`ToApi` helpers.

Tenant resolution lives in `ControllerBase` (`ResolveV2alpha3TenantRef`,
`GetTenantApiClientAsync`) and reads the **storage** version (`V2alpha3Tenant`) directly. The
tenant's Auth0 credentials come from `tenant.Spec.Auth` (domain + `secretRef` → a `V1Secret`
holding `clientId`/`clientSecret`).

## Naming conventions (`v2alpha3`)

The hub schema favors Kubernetes conventions over the Auth0 API's `snake_case`:

- **Field names** (`[JsonPropertyName]`): `camelCase` — `allowed_clients` → `allowedClients`,
  `google-oauth2` → `googleOauth2`.
- **Descriptive enum values** (`[JsonStringEnumMemberName]`): `camelCase` — `regular_web` →
  `regularWeb`, `access_token` → `accessToken`, `non_rotating` → `nonRotating`.
- **Meaningful external constants keep their canonical Auth0 wire form** — do NOT camelCase:
  - JOSE/JWA algorithms: `HS256`, `RS256`, `RS512`, `PS256`, `ES256`, `ES384`, `Ed25519`, `S256`
  - SAML/OAuth1 signature & digest algorithms: `rsa-sha1`, `rsa-sha256`, `sha256`, `RSA-SHA1`
  - SAML binding URNs: `urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST`
  - Protocol/API-version identifiers: `azure-active-directory-v1.0`,
    `microsoft-identity-platform-v2.0`, `v2026-1`

Rule of thumb: an enum value is a "meaningful constant" (keep Auth0 form) if it has uppercase
acronyms, `:`/`/`/`.`, or is a standard algorithm/version token. Otherwise it's a descriptive
label → `camelCase`.

**Why this is safe:** `[JsonStringEnumMemberName]` is only the CRD YAML token. Controllers
`switch` on the C# enum **member identifier** (unchanged) to produce the real Auth0 wire value,
so renaming the token never affects what Auth0 receives. To confirm a token's true wire value,
reflect the Auth0 SDK enum's nested `Values` constants (e.g.
`ConnectionSignatureAlgorithmEnumSaml.Values.RsaSha256 == "rsa-sha256"`).

Older versions are **frozen** — keep their original `snake_case`; never re-case a deprecated
served version (it would break existing manifests).

## Gotchas

- The codebase has some **pre-existing duplicate/dead enum types** (`X` vs `XEnum`, e.g.
  `ConnectionSamlSignatureAlgorithm` vs `ConnectionSignatureAlgorithmEnumSaml`). Check which one
  a property actually references before editing; the live one is usually the switch-mapped `...Enum`.
- The `Converters/Generated/<Kind>Copy.cs` files assign **every** model property in both
  `Convert` and `Revert`. When you add/remove a model property, update the Copy accordingly (a
  dropped assignment silently loses data — it won't fail to compile).

## Build & Test

```sh
dotnet build Alethic.Auth0.Operator.sln           # builds all + generates CRDs into config/
# The chart's RewriteCrd step can hit a transient file lock; just re-run the build.

# Tests use Microsoft.Testing.Platform — run the produced executable, not `dotnet test`:
src/Alethic.Auth0.Operator.Tests/bin/Debug/net10.0/Alethic.Auth0.Operator.Tests.exe
```

Mapping/round-trip tests live per Kind as `V2alpha3<Kind>ControllerMappingTests.cs` and
`V2alpha3ClientCopyTests.cs`. Reproduce tests when migrating; keep coverage.

## Adding a new storage version (e.g. `v2alpha4`)

Per Kind: clone the model tree into `Models/<Kind>/V2alpha4/` (rename type prefix + namespace,
apply the naming rules above); add the entity class (move the short name to it, strip from the
old); add controller + finalizer targeting the new version; retarget the `ConversionWebhook` to
the new hub and add converters (structural copy from the predecessor, plus retargeted reshaping
converters where they exist); clone the tests; update `ControllerBase` tenant resolution if the
Tenant hub moved; update `README.md`. Build, run tests, and inspect the transpiled CRDs to
confirm the new version is `storage: true`, older versions `served`, `conversion: Webhook`.
