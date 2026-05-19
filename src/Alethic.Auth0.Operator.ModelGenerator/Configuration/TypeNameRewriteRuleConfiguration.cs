namespace Alethic.Auth0.Operator.ModelGenerator.Configuration;

public sealed class TypeNameRewriteRuleConfiguration
{
    public required string Pattern { get; set; }

    public required string Replacement { get; set; }
}
