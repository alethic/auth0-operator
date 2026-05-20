using System.Reflection;
using System.Runtime.InteropServices;

using Alethic.Auth0.Operator.ModelGenerator.Configuration;

using Microsoft.CodeAnalysis;

namespace Alethic.Auth0.Operator.ModelGenerator.Discovery;

public sealed class AssemblyTypeDiscoverer
{
    private static readonly NullabilityInfoContext NullabilityContext = new();

    public DiscoveryResult Discover(string assemblyPath, GeneratorConfiguration configuration)
    {
        var fullAssemblyPath = Path.GetFullPath(assemblyPath);
        if (!File.Exists(fullAssemblyPath))
        {
            throw new FileNotFoundException($"Assembly '{fullAssemblyPath}' was not found.", fullAssemblyPath);
        }

        var resolver = new PathAssemblyResolver(GetResolverPaths(fullAssemblyPath, configuration));
        using var metadataLoadContext = new MetadataLoadContext(resolver);
        var assembly = metadataLoadContext.LoadFromAssemblyPath(fullAssemblyPath);

        var candidateTypes = assembly.DefinedTypes
            .Select(typeInfo => typeInfo.AsType())
            .Where(type => IsCandidate(type, configuration))
            .ToDictionary(type => type.FullName ?? type.Name, StringComparer.Ordinal);

        var selectedTypes = SelectTypes(candidateTypes, configuration);

        var discoveredTypes = selectedTypes
            .Select(MapType)
            .OrderBy(type => type.Namespace, StringComparer.Ordinal)
            .ThenBy(type => type.Name, StringComparer.Ordinal)
            .ToList();

        return new DiscoveryResult
        {
            AssemblyPath = fullAssemblyPath,
            AssemblyName = assembly.GetName().Name ?? Path.GetFileNameWithoutExtension(fullAssemblyPath),
            Types = discoveredTypes,
        };
    }

    private static IReadOnlyCollection<Type> SelectTypes(IReadOnlyDictionary<string, Type> candidateTypes, GeneratorConfiguration configuration)
    {
        if (configuration.RootTypeNames.Count == 0)
        {
            return [.. candidateTypes.Values];
        }

        var queue = new Queue<Type>(candidateTypes.Values.Where(type => MatchesRootType(type, configuration.RootTypeNames)));
        var selectedTypes = new Dictionary<string, Type>(StringComparer.Ordinal);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var currentKey = current.FullName ?? current.Name;
            if (!selectedTypes.TryAdd(currentKey, current))
            {
                continue;
            }

            if (!configuration.FollowReferencedTypes)
            {
                continue;
            }

            foreach (var referencedType in GetReferencedTypes(current, candidateTypes))
            {
                queue.Enqueue(referencedType);
            }
        }

        return [.. selectedTypes.Values];
    }

    private static IEnumerable<string> GetResolverPaths(string assemblyPath, GeneratorConfiguration configuration)
    {
        var assemblyDirectory = Path.GetDirectoryName(assemblyPath)!;

        return Directory.EnumerateFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll")
            .Concat(Directory.EnumerateFiles(assemblyDirectory, "*.dll"))
            .Concat(Directory.EnumerateFiles(assemblyDirectory, "*.exe"))
            .Concat(configuration.ResolverSearchDirectories.SelectMany(EnumerateResolverFiles))
            .Concat(configuration.ResolverAssemblyPaths.Where(File.Exists).Select(Path.GetFullPath))
            .Append(assemblyPath)
            .Distinct(StringComparer.OrdinalIgnoreCase);
    }

    private static IEnumerable<string> EnumerateResolverFiles(string directory)
    {
        var fullDirectory = Path.GetFullPath(directory);
        if (!Directory.Exists(fullDirectory))
        {
            return [];
        }

        return Directory.EnumerateFiles(fullDirectory, "*.dll", SearchOption.AllDirectories)
            .Concat(Directory.EnumerateFiles(fullDirectory, "*.exe", SearchOption.AllDirectories));
    }

    private static bool IsCandidate(Type type, GeneratorConfiguration configuration)
    {
        if (!type.IsPublic || type.IsNested)
        {
            return false;
        }

        if (!type.IsClass && !IsEnumLikeValueType(type))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(type.Namespace))
        {
            return false;
        }

        var fullName = type.FullName ?? type.Name;
        if (configuration.ExcludeTypeNames.Contains(type.Name, StringComparer.Ordinal)
            || configuration.ExcludeTypeNames.Contains(fullName, StringComparer.Ordinal))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(configuration.SourceNamespacePrefix)
            && !type.Namespace.StartsWith(configuration.SourceNamespacePrefix, StringComparison.Ordinal))
        {
            return false;
        }

        if (configuration.IncludeNamespaces.Count > 0
            && !configuration.IncludeNamespaces.Any(item => type.Namespace.StartsWith(item, StringComparison.Ordinal)))
        {
            return false;
        }

        if (configuration.SourceTypeNamePrefixes.Count > 0
            && !configuration.SourceTypeNamePrefixes.Any(prefix => type.Name.StartsWith(prefix, StringComparison.Ordinal)))
        {
            return false;
        }

        return true;
    }

    private static bool MatchesRootType(Type type, IReadOnlyCollection<string> rootTypeNames)
    {
        var fullName = type.FullName ?? type.Name;
        foreach (var rootTypeName in rootTypeNames)
        {
            if (rootTypeName.EndsWith('*'))
            {
                var prefix = rootTypeName[..^1];
                if (type.Name.StartsWith(prefix, StringComparison.Ordinal)
                    || fullName.StartsWith(prefix, StringComparison.Ordinal))
                {
                    return true;
                }

                continue;
            }

            if (string.Equals(type.Name, rootTypeName, StringComparison.Ordinal)
                || string.Equals(fullName, rootTypeName, StringComparison.Ordinal))
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<Type> GetReferencedTypes(Type type, IReadOnlyDictionary<string, Type> candidateTypes)
    {
        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
        {
            foreach (var referencedType in FlattenReferencedTypes(property.PropertyType))
            {
                var key = referencedType.FullName ?? referencedType.Name;
                if (candidateTypes.TryGetValue(key, out var candidateType))
                {
                    yield return candidateType;
                }
            }
        }
    }

    private static IEnumerable<Type> FlattenReferencedTypes(Type type)
    {
        if (type.IsByRef)
        {
            foreach (var referencedType in FlattenReferencedTypes(type.GetElementType()!))
            {
                yield return referencedType;
            }

            yield break;
        }

        if (type.IsArray)
        {
            foreach (var referencedType in FlattenReferencedTypes(type.GetElementType()!))
            {
                yield return referencedType;
            }

            yield break;
        }

        var nullableType = Nullable.GetUnderlyingType(type);
        if (nullableType is not null)
        {
            foreach (var referencedType in FlattenReferencedTypes(nullableType))
            {
                yield return referencedType;
            }

            yield break;
        }

        if (type.IsGenericType)
        {
            var genericTypeDefinition = type.GetGenericTypeDefinition();
            if (!string.Equals(genericTypeDefinition.FullName, "Auth0.ManagementApi.Core.Optional`1", StringComparison.Ordinal))
            {
                yield return genericTypeDefinition;
            }

            foreach (var genericArgument in type.GetGenericArguments())
            {
                foreach (var referencedType in FlattenReferencedTypes(genericArgument))
                {
                    yield return referencedType;
                }
            }

            yield break;
        }

        if ((type.IsClass || IsEnumLikeValueType(type)) && !string.IsNullOrWhiteSpace(type.Namespace))
        {
            yield return type;
        }
    }

    private static bool IsEnumLikeValueType(Type type)
    {
        return type.IsValueType
            && !type.IsPrimitive
            && !type.IsGenericType
            && !string.IsNullOrWhiteSpace(type.Namespace)
            && type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Length > 0;
    }

    private static string ConvertNameToSnakeCase(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        var builder = new System.Text.StringBuilder(name.Length + 8);
        for (var i = 0; i < name.Length; i++)
        {
            var current = name[i];
            if (char.IsUpper(current) && i > 0 && (char.IsLower(name[i - 1]) || (i + 1 < name.Length && char.IsLower(name[i + 1]))))
            {
                builder.Append('_');
            }

            builder.Append(char.ToLowerInvariant(current));
        }

        return builder.ToString();
    }

    private static DiscoveredType MapType(Type type)
    {
        var isEnumLike = IsEnumLikeValueType(type);
        var enumMembers = isEnumLike
            ? GetEnumLikeMembers(type)
            : [];

        return new DiscoveredType
        {
            Name = type.Name,
            Namespace = type.Namespace!,
            FullName = type.FullName ?? type.Name,
            IsEnumLike = isEnumLike,
            Attributes = [.. CustomAttributeData.GetCustomAttributes(type).Select(MapAttribute)],
            Properties = isEnumLike
                ? []
                : [.. type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Where(property => property.GetMethod?.IsPublic == true)
                    .OrderBy(property => property.MetadataToken)
                    .Select(MapProperty)],
            EnumMembers = enumMembers,
        };
    }

    private static List<DiscoveredEnumMember> GetEnumLikeMembers(Type type)
    {
        var valuesType = type.GetNestedType("Values", BindingFlags.Public | BindingFlags.NonPublic);
        if (valuesType is null)
        {
            return [.. type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .OrderBy(field => field.MetadataToken)
                .Select(field => new DiscoveredEnumMember
                {
                    Name = field.Name,
                    SerializedName = ConvertNameToSnakeCase(field.Name),
                })];
        }

        return [.. valuesType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(field => field.IsLiteral || field.IsStatic)
            .OrderBy(field => field.MetadataToken)
            .Select(MapEnumLikeMember)
            .OfType<DiscoveredEnumMember>()];
    }

    private static DiscoveredEnumMember? MapEnumLikeMember(FieldInfo field)
    {
        var serializedName = GetEnumLikeSerializedName(field);
        if (string.IsNullOrWhiteSpace(serializedName))
        {
            return null;
        }

        return new DiscoveredEnumMember
        {
            Name = field.Name,
            SerializedName = serializedName,
        };
    }

    private static string? GetEnumLikeSerializedName(FieldInfo field)
    {
        if (field.FieldType == typeof(string))
        {
            if (field.IsLiteral)
            {
                return field.GetRawConstantValue() as string;
            }

            try
            {
                return field.GetValue(null) as string;
            }
            catch
            {
                return null;
            }
        }

        return ConvertNameToSnakeCase(field.Name);
    }

    private static DiscoveredProperty MapProperty(PropertyInfo property)
    {
        var nullability = NullabilityContext.Create(property);

        return new DiscoveredProperty
        {
            Name = property.Name,
            Type = DiscoveredTypeReference.FromType(property.PropertyType, nullability),
            HasPublicSetter = property.SetMethod?.IsPublic == true,
            Attributes = [.. CustomAttributeData.GetCustomAttributes(property).Select(MapAttribute)],
        };
    }

    private static DiscoveredAttribute MapAttribute(CustomAttributeData attribute)
    {
        var attributeType = attribute.AttributeType;
        var shortTypeName = attributeType.Name.EndsWith("Attribute", StringComparison.Ordinal)
            ? attributeType.Name[..^9]
            : attributeType.Name;

        return new DiscoveredAttribute
        {
            TypeName = attributeType.FullName ?? attributeType.Name,
            ShortTypeName = shortTypeName,
            ConstructorArguments = [.. attribute.ConstructorArguments.Select(AttributeValueFormatter.Format)],
            NamedArguments = attribute.NamedArguments.ToDictionary(
                argument => argument.MemberName,
                argument => AttributeValueFormatter.Format(argument.TypedValue),
                StringComparer.Ordinal),
        };
    }
}