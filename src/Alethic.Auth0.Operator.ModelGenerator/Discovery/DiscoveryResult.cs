namespace Alethic.Auth0.Operator.ModelGenerator.Discovery;

public sealed class DiscoveryResult
{
    public required string AssemblyPath { get; init; }

    public required string AssemblyName { get; init; }

    public List<DiscoveredType> Types { get; init; } = [];
}