namespace Alethic.Auth0.Operator.ModelGenerator.Discovery;

public sealed class DiscoveredType
{
    public required string Name { get; init; }

    public required string Namespace { get; init; }

    public required string FullName { get; init; }

    public bool IsEnumLike { get; init; }

    public List<DiscoveredAttribute> Attributes { get; init; } = [];

    public List<DiscoveredProperty> Properties { get; init; } = [];

    public List<DiscoveredEnumMember> EnumMembers { get; init; } = [];
}