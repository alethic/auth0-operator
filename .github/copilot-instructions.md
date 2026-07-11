# Copilot Instructions

## Code Style
- When formatting C# code, wrap if statements onto two lines; do not use single-line if statements.

## Project Guidelines
- When migrating or renaming code, reproduce existing tests for the new implementation to preserve test coverage; do not delete the old tests outright.
- For Auth0 and special SAML connection option mappings, keep controller/converter conversions manual; do not replace them with JSON-based mapping or add JSON converters to model classes because source and target models can be incompatible. Never use JsonConvertTo for Auth0-to-model transformations; every Auth0 type → model type conversion must be mapped by hand. Prefer explicit, manual representations or alternative mapping approaches instead of attaching JSON converters to the models.
- Extract nested conversion logic in controller mappings into separate FromApi/ToApi helper methods instead of inlining them inside larger conversion methods.
  - Use explicit, manual mapping in these helpers for each nested type to preserve correctness and handle incompatibilities.
  - When guarding assignments for manual converters, check the source property in the if-condition and keep the ToApi(...) call on the assignment's right-hand side; do not bind the converted or source value in the if pattern.

### Generated Files / Code Generation
- Do not delete orphaned generated files when regenerating client V2alpha1 models unless explicitly requested.
- For client addon SharePoint external URLs, generate the V2alpha1 model property as string[] (always an array); do not use a union helper type for this field.