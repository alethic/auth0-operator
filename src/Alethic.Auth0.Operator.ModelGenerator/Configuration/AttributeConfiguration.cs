namespace Alethic.Auth0.Operator.ModelGenerator.Configuration;

public sealed class AttributeConfiguration
{
    public string TypeName { get; set; } = string.Empty;

    public List<string> ConstructorArguments { get; set; } = [];

    public Dictionary<string, string> NamedArguments { get; set; } = new(StringComparer.Ordinal);
}