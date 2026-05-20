# Alethic.Auth0.Operator.ModelGenerator

Refreshes copied Auth0 API model types by loading a source assembly, enumerating model classes, applying configurable transforms, and writing generated C# files.

## Usage

```powershell
dotnet run --project src/Alethic.Auth0.Operator.ModelGenerator -- \
  --assembly "C:\path\to\Auth0.ManagementApi.dll" \
  --output "D:\auth0-operator\artifacts\generated-models" \
	--config "src\Alethic.Auth0.Operator.ModelGenerator\connection-options-v2alpha1.json"
```

## Supported transforms

- Prefix generated type names.
- Remap source namespaces to target namespaces.
- Restrict discovery by namespace and type-name prefixes.
- Remove selected source attributes.
- Ignore selected properties globally or by source type.
- Add standard type or property attributes.
- Append standard properties to every generated type.

## Configuration workflow

Start from an existing scenario-specific config such as `connection-options-v2alpha1.json` and adjust the transform rules as needed.

You can also emit a fresh default file:

```powershell
dotnet run --project src/Alethic.Auth0.Operator.ModelGenerator -- --write-default-config --config ".\connection-options-v2alpha1.json"
```