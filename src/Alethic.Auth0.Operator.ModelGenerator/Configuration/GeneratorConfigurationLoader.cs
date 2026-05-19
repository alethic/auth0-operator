using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.ModelGenerator.Configuration;

public static class GeneratorConfigurationLoader
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
    };

    public static GeneratorConfiguration Load(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return GeneratorConfiguration.CreateDefault();
        }

        var fullPath = Path.GetFullPath(path);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"Configuration file '{fullPath}' was not found.", fullPath);
        }

        var json = File.ReadAllText(fullPath);
        return JsonSerializer.Deserialize<GeneratorConfiguration>(json, SerializerOptions)
            ?? GeneratorConfiguration.CreateDefault();
    }

    public static string Serialize(GeneratorConfiguration configuration)
    {
        return JsonSerializer.Serialize(configuration, SerializerOptions);
    }
}