namespace Alethic.Auth0.Operator.ModelGenerator.Transformation;

public sealed class GenerationResult
{
    public required string OutputDirectory { get; init; }

    public List<string> GeneratedFiles { get; init; } = [];
}