using System.Text.RegularExpressions;

using System.Text.Json.Serialization;

using Alethic.Auth0.Operator.ModelGenerator.Configuration;
using Alethic.Auth0.Operator.ModelGenerator.Discovery;

namespace Alethic.Auth0.Operator.ModelGenerator.Transformation;

public sealed class TypeTransformer
{
    public IReadOnlyList<GeneratedType> Transform(IReadOnlyList<DiscoveredType> discoveredTypes, GeneratorConfiguration configuration)
    {
        var filteredTypes = discoveredTypes
            .Where(type => !IsConfiguredTypeReplacement(type, configuration))
            .ToList();

        var generatedTypeMap = BuildGeneratedTypeMap(filteredTypes, configuration);
        return filteredTypes.Select(type => TransformType(type, configuration, generatedTypeMap)).ToList();
    }

    private GeneratedType TransformType(DiscoveredType discoveredType, GeneratorConfiguration configuration, IReadOnlyDictionary<string, DiscoveredTypeReference> generatedTypeMap)
    {
        var generatedTypeReference = generatedTypeMap[discoveredType.FullName];
        var generatedName = generatedTypeReference.Name;
        var generatedNamespace = generatedTypeReference.Namespace;

        var properties = discoveredType.Properties
            .Where(property => !ShouldIgnoreProperty(discoveredType, property, configuration))
            .Select(property => TransformProperty(property, configuration, generatedTypeMap))
            .Concat(configuration.StandardProperties.Select(TransformStandardProperty))
            .ToList();

        var attributes = discoveredType.Attributes
            .Where(attribute => !ShouldRemoveAttribute(attribute, configuration))
            .Select(MapAttribute)
            .Concat(configuration.AddedTypeAttributes.Select(MapAttribute))
            .ToList();

        var usingDirectives = CollectUsingDirectives(generatedNamespace, attributes, properties);

        return new GeneratedType
        {
            Name = generatedName,
            Namespace = generatedNamespace,
            IsEnumLike = discoveredType.IsEnumLike,
            Attributes = attributes,
            Properties = properties,
            EnumMembers = [.. discoveredType.EnumMembers.Select(member => (member.Name, member.SerializedName))],
            UsingDirectives = usingDirectives,
        };
    }

    private GeneratedProperty TransformProperty(DiscoveredProperty property, GeneratorConfiguration configuration, IReadOnlyDictionary<string, DiscoveredTypeReference> generatedTypeMap)
    {
        var transformedType = NormalizePropertyType(property.Type, generatedTypeMap, configuration, true);
        var jsonPropertyName = GetJsonPropertyName(property);
        var attributes = BuildPropertyAttributes(property, configuration, jsonPropertyName, transformedType);

        return new GeneratedProperty
        {
            Name = property.Name,
            JsonPropertyName = jsonPropertyName,
            Type = transformedType,
            HasSetter = property.HasPublicSetter,
            Attributes = attributes,
        };
    }

    private GeneratedProperty TransformStandardProperty(GeneratedPropertyConfiguration property)
    {
        var attributes = property.Attributes.Select(MapAttribute).ToList();
        if (!string.IsNullOrWhiteSpace(property.JsonPropertyName)
            && !attributes.Any(attribute => string.Equals(attribute.TypeName, typeof(JsonPropertyNameAttribute).FullName, StringComparison.Ordinal)))
        {
            attributes.Insert(0, new GeneratedAttribute
            {
                TypeName = nameof(JsonPropertyNameAttribute).Replace("Attribute", string.Empty),
                ConstructorArguments = [$"\"{property.JsonPropertyName}\""],
            });
        }

        return new GeneratedProperty
        {
            Name = property.Name,
            JsonPropertyName = property.JsonPropertyName,
            Summary = property.Summary,
            Type = MakeTypeNullable(ParseConfiguredType(property.TypeName)),
            HasSetter = true,
            Attributes = attributes,
        };
    }

    private static DiscoveredTypeReference ParseConfiguredType(string typeName)
    {
        var trimmed = typeName.Trim();
        var isNullable = trimmed.EndsWith("?", StringComparison.Ordinal);
        if (isNullable)
        {
            trimmed = trimmed[..^1];
        }

        var arrayRank = 0;
        while (trimmed.EndsWith("[]", StringComparison.Ordinal))
        {
            arrayRank++;
            trimmed = trimmed[..^2];
        }

        var aliasMap = new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["bool"] = typeof(bool).FullName!,
            ["byte"] = typeof(byte).FullName!,
            ["char"] = typeof(char).FullName!,
            ["decimal"] = typeof(decimal).FullName!,
            ["double"] = typeof(double).FullName!,
            ["float"] = typeof(float).FullName!,
            ["int"] = typeof(int).FullName!,
            ["long"] = typeof(long).FullName!,
            ["object"] = typeof(object).FullName!,
            ["sbyte"] = typeof(sbyte).FullName!,
            ["short"] = typeof(short).FullName!,
            ["string"] = typeof(string).FullName!,
            ["uint"] = typeof(uint).FullName!,
            ["ulong"] = typeof(ulong).FullName!,
            ["ushort"] = typeof(ushort).FullName!,
        };

        if (arrayRank > 0)
        {
            var elementType = ParseConfiguredType(trimmed);
            return new DiscoveredTypeReference
            {
                Name = elementType.Name,
                Namespace = elementType.Namespace,
                FullName = null,
                Alias = null,
                IsReferenceType = true,
                IsNullableReferenceType = isNullable,
                IsValueType = false,
                IsNullableValueType = false,
                IsOptionalWrapper = false,
                IsArray = true,
                ArrayRank = arrayRank,
                ElementType = elementType,
                GenericArguments = [],
            };
        }

        aliasMap.TryGetValue(trimmed, out var fullName);
        var namespaceSeparator = trimmed.LastIndexOf('.');

        return new DiscoveredTypeReference
        {
            Name = namespaceSeparator >= 0 ? trimmed[(namespaceSeparator + 1)..] : trimmed,
            Namespace = namespaceSeparator >= 0 ? trimmed[..namespaceSeparator] : string.Empty,
            FullName = fullName ?? (namespaceSeparator >= 0 ? trimmed : null),
            Alias = aliasMap.ContainsKey(trimmed) ? trimmed : null,
            IsReferenceType = !aliasMap.ContainsKey(trimmed) || trimmed is "string" or "object",
            IsNullableReferenceType = isNullable && (trimmed is "string" or "object" || namespaceSeparator >= 0),
            IsValueType = trimmed is "bool" or "byte" or "char" or "decimal" or "double" or "float" or "int" or "long" or "sbyte" or "short" or "uint" or "ulong" or "ushort",
            IsNullableValueType = isNullable && trimmed != "string" && !trimmed.EndsWith("]", StringComparison.Ordinal),
        };
    }

    private static bool IsConfiguredTypeReplacement(DiscoveredType discoveredType, GeneratorConfiguration configuration)
    {
        return TryGetConfiguredTypeReplacement(discoveredType.FullName, discoveredType.Name, configuration, out _);
    }

    private static Dictionary<string, DiscoveredTypeReference> BuildGeneratedTypeMap(IReadOnlyList<DiscoveredType> discoveredTypes, GeneratorConfiguration configuration)
    {
        var map = new Dictionary<string, DiscoveredTypeReference>(StringComparer.Ordinal);
        foreach (var discoveredType in discoveredTypes)
        {
            var rewrittenName = RewriteTypeName(StripSourcePrefixes(discoveredType.Name, configuration), configuration);
            map[discoveredType.FullName] = new DiscoveredTypeReference
            {
                Name = configuration.ClassPrefix + rewrittenName,
                Namespace = MapNamespace(discoveredType.Namespace, configuration),
                FullName = discoveredType.FullName,
                Alias = null,
                IsReferenceType = true,
                IsNullableReferenceType = false,
                IsValueType = false,
                IsNullableValueType = false,
                IsOptionalWrapper = false,
                IsArray = false,
                ArrayRank = 0,
            };
        }

        return map;
    }

    private static List<GeneratedAttribute> BuildPropertyAttributes(DiscoveredProperty property, GeneratorConfiguration configuration, string? jsonPropertyName, DiscoveredTypeReference transformedType)
    {
        var attributes = property.Attributes
            .Where(attribute => !ShouldRemoveAttribute(attribute, configuration))
            .Select(MapAttribute)
            .Concat(configuration.AddedPropertyAttributes.Select(MapAttribute))
            .ToList();

        if (!string.IsNullOrWhiteSpace(jsonPropertyName)
            && !attributes.Any(attribute => string.Equals(attribute.TypeName, nameof(JsonPropertyNameAttribute).Replace("Attribute", string.Empty), StringComparison.Ordinal)
                || string.Equals(attribute.TypeName, typeof(JsonPropertyNameAttribute).FullName, StringComparison.Ordinal)))
        {
            attributes.Insert(0, new GeneratedAttribute
            {
                TypeName = nameof(JsonPropertyNameAttribute).Replace("Attribute", string.Empty),
                ConstructorArguments = [$"\"{jsonPropertyName}\""],
            });
        }

        if (ShouldAddNullWhenWritingNull(property, transformedType)
            && !attributes.Any(IsWhenWritingNullJsonIgnoreAttribute))
        {
            attributes.Add(new GeneratedAttribute
            {
                TypeName = nameof(JsonIgnoreAttribute).Replace("Attribute", string.Empty),
                NamedArguments = new Dictionary<string, string>(StringComparer.Ordinal)
                {
                    ["Condition"] = "JsonIgnoreCondition.WhenWritingNull",
                },
            });
        }

        return attributes;
    }

    private static GeneratedAttribute MapAttribute(DiscoveredAttribute attribute)
    {
        return new GeneratedAttribute
        {
            TypeName = attribute.ShortTypeName,
            ConstructorArguments = [.. attribute.ConstructorArguments],
            NamedArguments = new Dictionary<string, string>(attribute.NamedArguments, StringComparer.Ordinal),
        };
    }

    private static GeneratedAttribute MapAttribute(AttributeConfiguration attribute)
    {
        return new GeneratedAttribute
        {
            TypeName = attribute.TypeName,
            ConstructorArguments = [.. attribute.ConstructorArguments],
            NamedArguments = new Dictionary<string, string>(attribute.NamedArguments, StringComparer.Ordinal),
        };
    }

    private static bool ShouldIgnoreProperty(DiscoveredType discoveredType, DiscoveredProperty property, GeneratorConfiguration configuration)
    {
        if (string.Equals(property.Name, "AdditionalProperties", StringComparison.Ordinal)
            || property.Attributes.Any(attribute =>
                string.Equals(attribute.TypeName, typeof(JsonExtensionDataAttribute).FullName, StringComparison.Ordinal)
                || string.Equals(attribute.ShortTypeName, nameof(JsonExtensionDataAttribute).Replace("Attribute", string.Empty), StringComparison.Ordinal)))
        {
            return true;
        }

        if (configuration.IgnoredPropertyNames.Contains(property.Name, StringComparer.Ordinal))
        {
            return true;
        }

        return configuration.IgnoredPropertiesByType.TryGetValue(discoveredType.Name, out var ignoredProperties)
            && ignoredProperties.Contains(property.Name, StringComparer.Ordinal);
    }

    private static bool ShouldRemoveAttribute(DiscoveredAttribute attribute, GeneratorConfiguration configuration)
    {
        var candidates = new[]
        {
            attribute.TypeName,
            attribute.ShortTypeName,
            attribute.ShortTypeName + "Attribute",
        };

        return candidates.Any(candidate => configuration.RemovedAttributeTypeNames.Contains(candidate, StringComparer.Ordinal));
    }

    private static string? GetJsonPropertyName(DiscoveredProperty property)
    {
        var attribute = property.Attributes.FirstOrDefault(attribute =>
            string.Equals(attribute.TypeName, typeof(JsonPropertyNameAttribute).FullName, StringComparison.Ordinal)
            || string.Equals(attribute.ShortTypeName, nameof(JsonPropertyNameAttribute).Replace("Attribute", string.Empty), StringComparison.Ordinal));

        return attribute?.ConstructorArguments.FirstOrDefault()?.Trim('"');
    }

    private static bool ShouldAddNullWhenWritingNull(DiscoveredProperty property, DiscoveredTypeReference transformedType)
    {
        return transformedType.IsOptionalWrapper
            || transformedType.IsNullableValueType
            || transformedType.IsNullableReferenceType
            || property.Attributes.Any(attribute =>
                string.Equals(attribute.TypeName, "System.Runtime.CompilerServices.NullableAttribute", StringComparison.Ordinal)
                || string.Equals(attribute.ShortTypeName, "Nullable", StringComparison.Ordinal)
                || string.Equals(attribute.TypeName, "Auth0.ManagementApi.Core.OptionalAttribute", StringComparison.Ordinal)
                || string.Equals(attribute.ShortTypeName, "Optional", StringComparison.Ordinal));
    }

    private static bool IsWhenWritingNullJsonIgnoreAttribute(GeneratedAttribute attribute)
    {
        return (string.Equals(attribute.TypeName, nameof(JsonIgnoreAttribute).Replace("Attribute", string.Empty), StringComparison.Ordinal)
                || string.Equals(attribute.TypeName, typeof(JsonIgnoreAttribute).FullName, StringComparison.Ordinal))
            && attribute.NamedArguments.TryGetValue("Condition", out var condition)
            && string.Equals(condition, "JsonIgnoreCondition.WhenWritingNull", StringComparison.Ordinal);
    }

    private static bool IsNullableValueTypeReference(DiscoveredTypeReference type)
    {
        return string.Equals(type.FullName, "System.Nullable`1", StringComparison.Ordinal)
            || string.Equals(type.Name, "Nullable", StringComparison.Ordinal);
    }

    private static bool IsEnumerableTypeReference(DiscoveredTypeReference type)
    {
        return string.Equals(type.FullName, "System.Collections.Generic.IEnumerable`1", StringComparison.Ordinal)
            || string.Equals(type.Namespace, "System.Collections.Generic", StringComparison.Ordinal)
            && string.Equals(type.Name, "IEnumerable", StringComparison.Ordinal);
    }

    private static DiscoveredTypeReference NormalizePropertyType(DiscoveredTypeReference type, IReadOnlyDictionary<string, DiscoveredTypeReference> generatedTypeMap, GeneratorConfiguration configuration, bool makeNullable)
    {
        if (IsNullableValueTypeReference(type) && type.GenericArguments.Count == 1)
        {
            var underlyingType = NormalizePropertyType(type.GenericArguments[0], generatedTypeMap, configuration, false);
            return new DiscoveredTypeReference
            {
                Name = underlyingType.Name,
                Namespace = underlyingType.Namespace,
                FullName = underlyingType.FullName,
                Alias = underlyingType.Alias,
                IsReferenceType = underlyingType.IsReferenceType,
                IsNullableReferenceType = underlyingType.IsNullableReferenceType,
                IsValueType = underlyingType.IsValueType,
                IsNullableValueType = true,
                IsOptionalWrapper = type.IsOptionalWrapper || underlyingType.IsOptionalWrapper,
                IsArray = underlyingType.IsArray,
                ArrayRank = underlyingType.ArrayRank,
                ElementType = underlyingType.ElementType,
                GenericArguments = [.. underlyingType.GenericArguments],
            };
        }

        if (IsEnumerableTypeReference(type) && type.GenericArguments.Count == 1)
        {
            var elementType = NormalizePropertyType(type.GenericArguments[0], generatedTypeMap, configuration, false);
            return new DiscoveredTypeReference
            {
                Name = elementType.Name,
                Namespace = elementType.Namespace,
                FullName = elementType.FullName,
                Alias = null,
                IsReferenceType = true,
                IsNullableReferenceType = makeNullable,
                IsValueType = false,
                IsNullableValueType = false,
                IsOptionalWrapper = type.IsOptionalWrapper || elementType.IsOptionalWrapper,
                IsArray = true,
                ArrayRank = 1,
                ElementType = elementType,
                GenericArguments = [],
            };
        }

        if (type.IsArray && type.ElementType is not null)
        {
            return new DiscoveredTypeReference
            {
                Name = type.Name,
                Namespace = type.Namespace,
                FullName = type.FullName,
                Alias = type.Alias,
                IsReferenceType = true,
                IsNullableReferenceType = makeNullable,
                IsValueType = false,
                IsNullableValueType = false,
                IsOptionalWrapper = type.IsOptionalWrapper,
                IsArray = true,
                ArrayRank = type.ArrayRank,
                ElementType = NormalizePropertyType(type.ElementType, generatedTypeMap, configuration, false),
                GenericArguments = [.. type.GenericArguments.Select(argument => NormalizePropertyType(argument, generatedTypeMap, configuration, false))],
            };
        }

        if (TryGetConfiguredTypeReplacement(type.FullName, type.Name, configuration, out var replacementTypeName))
        {
            var replacementType = ParseConfiguredType(replacementTypeName!);
            return makeNullable
                ? MakeTypeNullable(replacementType)
                : replacementType;
        }

        if (!string.IsNullOrWhiteSpace(type.FullName) && generatedTypeMap.TryGetValue(type.FullName, out var generatedType))
        {
            return new DiscoveredTypeReference
            {
                Name = generatedType.Name,
                Namespace = generatedType.Namespace,
                FullName = generatedType.FullName,
                Alias = null,
                IsReferenceType = true,
                IsNullableReferenceType = makeNullable || type.IsOptionalWrapper,
                IsValueType = false,
                IsNullableValueType = false,
                IsOptionalWrapper = type.IsOptionalWrapper,
                IsArray = false,
                ArrayRank = 0,
                GenericArguments = [.. type.GenericArguments.Select(argument => NormalizePropertyType(argument, generatedTypeMap, configuration, false))],
            };
        }

        return new DiscoveredTypeReference
        {
            Name = type.Name,
            Namespace = type.Namespace,
            FullName = type.FullName,
            Alias = type.Alias,
            IsReferenceType = type.IsReferenceType,
            IsNullableReferenceType = type.IsReferenceType && (makeNullable || type.IsNullableReferenceType || type.IsOptionalWrapper),
            IsValueType = type.IsValueType,
            IsNullableValueType = type.IsNullableValueType || (type.IsValueType && (makeNullable || type.IsOptionalWrapper)),
            IsOptionalWrapper = type.IsOptionalWrapper,
            IsArray = type.IsArray,
            ArrayRank = type.ArrayRank,
            ElementType = type.ElementType is null ? null : NormalizePropertyType(type.ElementType, generatedTypeMap, configuration, false),
            GenericArguments = [.. type.GenericArguments.Select(argument => NormalizePropertyType(argument, generatedTypeMap, configuration, false))],
        };
    }

    private static bool TryGetConfiguredTypeReplacement(string? fullName, string name, GeneratorConfiguration configuration, out string? replacementTypeName)
    {
        replacementTypeName = null;

        if (configuration.TypeReplacements.Count == 0)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(fullName) && configuration.TypeReplacements.TryGetValue(fullName, out replacementTypeName))
        {
            return true;
        }

        return configuration.TypeReplacements.TryGetValue(name, out replacementTypeName);
    }

    private static DiscoveredTypeReference MakeTypeNullable(DiscoveredTypeReference type)
    {
        return new DiscoveredTypeReference
        {
            Name = type.Name,
            Namespace = type.Namespace,
            FullName = type.FullName,
            Alias = type.Alias,
            IsReferenceType = type.IsReferenceType,
            IsNullableReferenceType = type.IsReferenceType || type.IsArray || type.IsNullableReferenceType,
            IsValueType = type.IsValueType,
            IsNullableValueType = type.IsNullableValueType || type.IsValueType,
            IsOptionalWrapper = type.IsOptionalWrapper,
            IsArray = type.IsArray,
            ArrayRank = type.ArrayRank,
            ElementType = type.ElementType,
            GenericArguments = [.. type.GenericArguments],
        };
    }

    private static string StripSourcePrefixes(string name, GeneratorConfiguration configuration)
    {
        foreach (var prefix in configuration.SourceTypeNamePrefixes.OrderByDescending(prefix => prefix.Length))
        {
            if (name.StartsWith(prefix, StringComparison.Ordinal))
            {
                return name[prefix.Length..];
            }
        }

        return name;
    }

    private static string RewriteTypeName(string name, GeneratorConfiguration configuration)
    {
        var rewrittenName = name;
        foreach (var rule in configuration.TypeNameRewriteRules)
        {
            rewrittenName = Regex.Replace(rewrittenName, rule.Pattern, rule.Replacement, RegexOptions.CultureInvariant);
        }

        return rewrittenName;
    }

    private static string MapNamespace(string sourceNamespace, GeneratorConfiguration configuration)
    {
        if (configuration.NamespaceMappings.TryGetValue(sourceNamespace, out var mappedNamespace))
        {
            return mappedNamespace;
        }

        if (!string.IsNullOrWhiteSpace(configuration.SourceNamespacePrefix)
            && !string.IsNullOrWhiteSpace(configuration.TargetNamespacePrefix)
            && sourceNamespace.StartsWith(configuration.SourceNamespacePrefix, StringComparison.Ordinal))
        {
            return configuration.TargetNamespacePrefix + sourceNamespace[configuration.SourceNamespacePrefix.Length..];
        }

        return string.IsNullOrWhiteSpace(configuration.TargetNamespacePrefix)
            ? sourceNamespace
            : configuration.TargetNamespacePrefix;
    }

    private static List<string> CollectUsingDirectives(string generatedNamespace, List<GeneratedAttribute> attributes, List<GeneratedProperty> properties)
    {
        var namespaces = new HashSet<string>(StringComparer.Ordinal)
        {
            "System.Text.Json.Serialization",
        };

        foreach (var attribute in attributes)
        {
            TryAddAttributeNamespace(attribute, namespaces);
        }

        foreach (var property in properties)
        {
            AddTypeNamespaces(property.Type, namespaces, generatedNamespace);
            foreach (var attribute in property.Attributes)
            {
                TryAddAttributeNamespace(attribute, namespaces);
            }
        }

        return namespaces.OrderBy(name => name, StringComparer.Ordinal).ToList();
    }

    private static void TryAddAttributeNamespace(GeneratedAttribute attribute, HashSet<string> namespaces)
    {
        if (attribute.TypeName.Contains('.', StringComparison.Ordinal))
        {
            var separator = attribute.TypeName.LastIndexOf('.');
            if (separator > 0)
            {
                namespaces.Add(attribute.TypeName[..separator]);
            }
        }
        else if (attribute.TypeName is "JsonPropertyName" or "JsonIgnore")
        {
            namespaces.Add("System.Text.Json.Serialization");
        }
    }

    private static void AddTypeNamespaces(DiscoveredTypeReference type, HashSet<string> namespaces, string generatedNamespace)
    {
        if (!string.IsNullOrWhiteSpace(type.Namespace)
            && !string.Equals(type.Namespace, generatedNamespace, StringComparison.Ordinal)
            && string.IsNullOrWhiteSpace(type.Alias))
        {
            namespaces.Add(type.Namespace);
        }

        if (type.IsArray && type.ElementType is not null)
        {
            AddTypeNamespaces(type.ElementType, namespaces, generatedNamespace);
        }

        foreach (var argument in type.GenericArguments)
        {
            AddTypeNamespaces(argument, namespaces, generatedNamespace);
        }
    }
}