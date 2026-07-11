namespace Alethic.Auth0.Operator.ModelGenerator.Transformation;

public sealed class GeneratedType
{
    public required string Name { get; init; }

    public required string Namespace { get; init; }

    public bool IsEnumLike { get; init; }

    public List<GeneratedAttribute> Attributes { get; init; } = [];

    public List<GeneratedProperty> Properties { get; init; } = [];

    public List<(string Name, string SerializedName)> EnumMembers { get; init; } = [];

    public List<string> UsingDirectives { get; init; } = [];
}