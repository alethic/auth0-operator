namespace Alethic.Auth0.Operator.ModelGenerator.CommandLine;

public sealed class GeneratorOptions
{
    public string? AssemblyPath { get; set; }

    public string? OutputDirectory { get; set; }

    public string? ConfigurationPath { get; set; }

    public string? ClassPrefix { get; set; }

    public string? SourceNamespacePrefix { get; set; }

    public string? TargetNamespacePrefix { get; set; }

    public bool? OverwriteExistingFiles { get; set; }

    public bool ShowHelp { get; set; }

    public bool WriteDefaultConfiguration { get; set; }
}