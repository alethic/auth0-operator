using Alethic.Auth0.Operator.ModelGenerator.CommandLine;
using Alethic.Auth0.Operator.ModelGenerator.Configuration;
using Alethic.Auth0.Operator.ModelGenerator.Discovery;
using Alethic.Auth0.Operator.ModelGenerator.Generation;
using Alethic.Auth0.Operator.ModelGenerator.Transformation;

namespace Alethic.Auth0.Operator.ModelGenerator;

internal static class Program
{
    private static int Main(string[] args)
    {
        try
        {
            var options = CommandLineParser.Parse(args);
            if (options.ShowHelp)
            {
                Console.WriteLine(CommandLineParser.GetUsage());
                return 0;
            }

            var configuration = GeneratorConfigurationLoader.Load(options.ConfigurationPath)
                .ApplyOverrides(options);

            if (options.WriteDefaultConfiguration)
            {
                var path = GetDefaultConfigurationOutputPath(options.ConfigurationPath);
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                File.WriteAllText(path, GeneratorConfigurationLoader.Serialize(configuration));
                Console.WriteLine($"Wrote default configuration to '{path}'.");
                return 0;
            }

            CommandLineParser.ValidateForExecution(options);

            var discoverer = new AssemblyTypeDiscoverer();
            var discoveryResult = discoverer.Discover(options.AssemblyPath!, configuration);
            var transformer = new TypeTransformer();
            var generatedTypes = transformer.Transform(discoveryResult.Types, configuration);
            var writer = new RoslynCodeWriter();
            var generationResult = writer.WriteFiles(generatedTypes, options.OutputDirectory!, configuration);

            Console.WriteLine($"Assembly: {Path.GetFullPath(options.AssemblyPath!)}");
            Console.WriteLine($"Output: {Path.GetFullPath(options.OutputDirectory!)}");
            Console.WriteLine($"Prefix: {configuration.ClassPrefix}");
            Console.WriteLine($"Discovered {discoveryResult.Types.Count} type(s) in {discoveryResult.AssemblyName}.");
            Console.WriteLine($"Generated {generationResult.GeneratedFiles.Count} file(s).");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 1;
        }
    }

    private static string GetDefaultConfigurationOutputPath(string? configuredPath)
    {
        if (!string.IsNullOrWhiteSpace(configuredPath))
        {
            return Path.GetFullPath(configuredPath);
        }

        return Path.Combine(Environment.CurrentDirectory, "auth0-model-generator.json");
    }
}
