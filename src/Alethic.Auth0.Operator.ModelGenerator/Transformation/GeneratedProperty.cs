using Alethic.Auth0.Operator.ModelGenerator.Discovery;

namespace Alethic.Auth0.Operator.ModelGenerator.Transformation;

public sealed class GeneratedProperty
{
    public required string Name { get; init; }

    public string? JsonPropertyName { get; init; }

    public string? Summary { get; init; }

    public required DiscoveredTypeReference Type { get; init; }

    public bool HasSetter { get; init; } = true;

    public List<GeneratedAttribute> Attributes { get; init; } = [];
}