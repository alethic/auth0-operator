namespace Alethic.Auth0.Operator.ModelGenerator.Discovery;

public sealed class DiscoveredProperty
{
    public required string Name { get; init; }

    public required DiscoveredTypeReference Type { get; init; }

    public bool HasPublicSetter { get; init; }

    public List<DiscoveredAttribute> Attributes { get; init; } = [];
}