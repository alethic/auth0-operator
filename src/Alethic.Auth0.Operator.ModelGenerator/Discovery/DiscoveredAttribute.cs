namespace Alethic.Auth0.Operator.ModelGenerator.Discovery;

public sealed class DiscoveredAttribute
{
    public required string TypeName { get; init; }

    public required string ShortTypeName { get; init; }

    public List<string> ConstructorArguments { get; init; } = [];

    public Dictionary<string, string> NamedArguments { get; init; } = new(StringComparer.Ordinal);
}