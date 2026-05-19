namespace Alethic.Auth0.Operator.ModelGenerator.CommandLine;

public static class CommandLineParser
{
    public static GeneratorOptions Parse(string[] args)
    {
        var options = new GeneratorOptions();

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "-h":
                case "--help":
                case "/?":
                    options.ShowHelp = true;
                    break;
                case "--write-default-config":
                    options.WriteDefaultConfiguration = true;
                    break;
                case "--assembly":
                    options.AssemblyPath = ReadValue(args, ref i, arg);
                    break;
                case "--output":
                    options.OutputDirectory = ReadValue(args, ref i, arg);
                    break;
                case "--config":
                    options.ConfigurationPath = ReadValue(args, ref i, arg);
                    break;
                case "--prefix":
                    options.ClassPrefix = ReadValue(args, ref i, arg);
                    break;
                case "--source-namespace-prefix":
                    options.SourceNamespacePrefix = ReadValue(args, ref i, arg);
                    break;
                case "--target-namespace-prefix":
                    options.TargetNamespacePrefix = ReadValue(args, ref i, arg);
                    break;
                case "--overwrite":
                    options.OverwriteExistingFiles = true;
                    break;
                case "--no-overwrite":
                    options.OverwriteExistingFiles = false;
                    break;
                default:
                    throw new ArgumentException($"Unknown argument '{arg}'.");
            }
        }

        return options;
    }

    public static string GetUsage()
    {
        return string.Join(Environment.NewLine, new[]
        {
            "Usage:",
            "  dotnet run --project src/Alethic.Auth0.Operator.ModelGenerator -- --assembly <path> --output <directory> [options]",
            string.Empty,
            "Options:",
            "  --assembly <path>                  Path to the source Auth0 assembly.",
            "  --output <directory>               Output directory for generated .cs files.",
            "  --config <path>                    Optional JSON configuration file.",
            "  --prefix <value>                   Optional class name prefix override.",
            "  --source-namespace-prefix <value>  Optional source namespace prefix override.",
            "  --target-namespace-prefix <value>  Optional target namespace prefix override.",
            "  --overwrite                        Overwrite existing generated files.",
            "  --no-overwrite                     Keep existing generated files.",
            "  --write-default-config             Write a default config file to the config path or current directory.",
            "  -h|--help                          Show help.",
        });
    }

    public static void ValidateForExecution(GeneratorOptions options)
    {
        if (options.ShowHelp || options.WriteDefaultConfiguration)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(options.AssemblyPath))
        {
            throw new ArgumentException("The --assembly option is required.");
        }

        if (string.IsNullOrWhiteSpace(options.OutputDirectory))
        {
            throw new ArgumentException("The --output option is required.");
        }
    }

    private static string ReadValue(string[] args, ref int index, string optionName)
    {
        if (index + 1 >= args.Length)
        {
            throw new ArgumentException($"Missing value for '{optionName}'.");
        }

        index++;
        return args[index];
    }
}