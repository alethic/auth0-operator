namespace Alethic.Auth0.Operator.ModelGenerator.Configuration;

public sealed class GeneratorConfiguration
{
    public string ClassPrefix { get; set; } = "Generated";

    public string SourceNamespacePrefix { get; set; } = "Auth0.ManagementApi.Models";

    public string TargetNamespacePrefix { get; set; } = "Alethic.Auth0.Generated.Models";

    public List<string> RootTypeNames { get; set; } = [];

    public bool FollowReferencedTypes { get; set; }

    public List<string> ResolverSearchDirectories { get; set; } = [];

    public List<string> ResolverAssemblyPaths { get; set; } = [];

    public List<string> SourceTypeNamePrefixes { get; set; } = ["Create", "Update"];

    public List<string> IncludeNamespaces { get; set; } = ["Auth0.ManagementApi.Models"];

    public List<string> ExcludeTypeNames { get; set; } = [];

    public List<string> IgnoredPropertyNames { get; set; } = [];

    public Dictionary<string, List<string>> IgnoredPropertiesByType { get; set; } = new(StringComparer.Ordinal);

    public List<string> RemovedAttributeTypeNames { get; set; } =
    [
        "JsonConverterAttribute",
        "Newtonsoft.Json.JsonConverterAttribute",
        "IsReadOnlyAttribute",
        "System.Runtime.CompilerServices.IsReadOnlyAttribute",
        "RequiredAttribute",
        "System.ComponentModel.DataAnnotations.RequiredAttribute",
    ];

    public List<AttributeConfiguration> AddedTypeAttributes { get; set; } = [];

    public List<AttributeConfiguration> AddedPropertyAttributes { get; set; } = [];

    public List<GeneratedPropertyConfiguration> StandardProperties { get; set; } = [];

    public Dictionary<string, string> NamespaceMappings { get; set; } = new(StringComparer.Ordinal);

    public bool EmitRecords { get; set; } = true;

    public bool UseNamespaceSubdirectories { get; set; } = true;

    public bool OverwriteExistingFiles { get; set; } = true;

    public static GeneratorConfiguration CreateDefault() => new()
    {
        StandardProperties =
        [
            new GeneratedPropertyConfiguration
            {
                Name = "SourceModelType",
                TypeName = "string?",
                Summary = "Fully qualified source model type name.",
                JsonPropertyName = "sourceModelType",
                Attributes =
                [
                    new AttributeConfiguration
                    {
                        TypeName = "JsonIgnore",
                        NamedArguments = new Dictionary<string, string>(StringComparer.Ordinal)
                        {
                            ["Condition"] = "JsonIgnoreCondition.WhenWritingNull",
                        },
                    },
                ],
            },
        ],
    };

    public GeneratorConfiguration ApplyOverrides(CommandLine.GeneratorOptions options)
    {
        return new GeneratorConfiguration
        {
            ClassPrefix = string.IsNullOrWhiteSpace(options.ClassPrefix) ? ClassPrefix : options.ClassPrefix,
            SourceNamespacePrefix = string.IsNullOrWhiteSpace(options.SourceNamespacePrefix) ? SourceNamespacePrefix : options.SourceNamespacePrefix,
            TargetNamespacePrefix = string.IsNullOrWhiteSpace(options.TargetNamespacePrefix) ? TargetNamespacePrefix : options.TargetNamespacePrefix,
            RootTypeNames = [.. RootTypeNames],
            FollowReferencedTypes = FollowReferencedTypes,
            ResolverSearchDirectories = [.. ResolverSearchDirectories],
            ResolverAssemblyPaths = [.. ResolverAssemblyPaths],
            SourceTypeNamePrefixes = [.. SourceTypeNamePrefixes],
            IncludeNamespaces = [.. IncludeNamespaces],
            ExcludeTypeNames = [.. ExcludeTypeNames],
            IgnoredPropertyNames = [.. IgnoredPropertyNames],
            IgnoredPropertiesByType = new Dictionary<string, List<string>>(IgnoredPropertiesByType, StringComparer.Ordinal),
            RemovedAttributeTypeNames = [.. RemovedAttributeTypeNames],
            AddedTypeAttributes = [.. AddedTypeAttributes],
            AddedPropertyAttributes = [.. AddedPropertyAttributes],
            StandardProperties = [.. StandardProperties],
            NamespaceMappings = new Dictionary<string, string>(NamespaceMappings, StringComparer.Ordinal),
            EmitRecords = EmitRecords,
            UseNamespaceSubdirectories = UseNamespaceSubdirectories,
            OverwriteExistingFiles = options.OverwriteExistingFiles ?? OverwriteExistingFiles,
        };
    }
}