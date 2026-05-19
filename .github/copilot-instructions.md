# Copilot Instructions

## Code Style
- When formatting C# code, wrap if statements onto two lines; do not use single-line if statements.

## Project Guidelines
- For Auth0 and special SAML connection option mappings, keep controller/converter conversions manual; do not replace them with JSON-based mapping or add JSON converters to model classes because source and target models can be incompatible. Prefer explicit representations or alternative mapping approaches instead of attaching JSON converters to the models.
- Extract nested conversion logic in controller mappings into separate FromApi/ToApi helper methods instead of inlining them inside larger conversion methods.
  - Use explicit, manual mapping in these helpers for each nested type to preserve correctness and handle incompatibilities.
  - When guarding assignments for manual converters, check the source property in the if-condition and keep the ToApi(...) call on the assignment's right-hand side; do not bind the converted or source value in the if pattern.