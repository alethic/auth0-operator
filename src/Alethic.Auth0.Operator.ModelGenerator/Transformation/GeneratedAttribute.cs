namespace Alethic.Auth0.Operator.ModelGenerator.Transformation;

public sealed class GeneratedAttribute
{
    public required string TypeName { get; init; }

    public List<string> ConstructorArguments { get; init; } = [];

    public Dictionary<string, string> NamedArguments { get; init; } = new(StringComparer.Ordinal);
}