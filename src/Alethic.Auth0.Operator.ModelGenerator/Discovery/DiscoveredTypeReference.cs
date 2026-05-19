namespace Alethic.Auth0.Operator.ModelGenerator.Discovery;

public sealed class DiscoveredTypeReference
{
    public const string OptionalGenericTypeFullName = "Auth0.ManagementApi.Core.Optional`1";

    public required string Name { get; init; }

    public string Namespace { get; init; } = string.Empty;

    public string? FullName { get; init; }

    public string? Alias { get; init; }

    public bool IsReferenceType { get; init; }

    public bool IsNullableReferenceType { get; init; }

    public bool IsValueType { get; init; }

    public bool IsNullableValueType { get; init; }

    public bool IsOptionalWrapper { get; init; }

    public bool IsArray { get; init; }

    public int ArrayRank { get; init; }

    public DiscoveredTypeReference? ElementType { get; init; }

    public List<DiscoveredTypeReference> GenericArguments { get; init; } = [];

    public static DiscoveredTypeReference FromType(Type type)
    {
        if (type.IsByRef)
        {
            return FromType(type.GetElementType()!);
        }

        if (type.IsArray)
        {
            return new DiscoveredTypeReference
            {
                Name = type.Name,
                Namespace = type.Namespace ?? string.Empty,
                FullName = type.FullName,
                Alias = null,
                IsReferenceType = true,
                IsNullableReferenceType = false,
                IsValueType = false,
                IsNullableValueType = false,
                IsOptionalWrapper = false,
                IsArray = true,
                ArrayRank = type.GetArrayRank(),
                ElementType = FromType(type.GetElementType()!),
            };
        }

        var underlyingNullableType = Nullable.GetUnderlyingType(type);
        if (underlyingNullableType is not null)
        {
            var nullableTypeReference = FromType(underlyingNullableType);
            return new DiscoveredTypeReference
            {
                Name = nullableTypeReference.Name,
                Namespace = nullableTypeReference.Namespace,
                FullName = nullableTypeReference.FullName,
                Alias = nullableTypeReference.Alias,
                IsReferenceType = nullableTypeReference.IsReferenceType,
                IsNullableReferenceType = nullableTypeReference.IsNullableReferenceType,
                IsValueType = nullableTypeReference.IsValueType,
                IsNullableValueType = true,
                IsOptionalWrapper = nullableTypeReference.IsOptionalWrapper,
                IsArray = nullableTypeReference.IsArray,
                ArrayRank = nullableTypeReference.ArrayRank,
                ElementType = nullableTypeReference.ElementType,
                GenericArguments = [.. nullableTypeReference.GenericArguments],
            };
        }

        if (type.IsGenericParameter)
        {
            return new DiscoveredTypeReference
            {
                Name = type.Name,
                Namespace = string.Empty,
                FullName = type.Name,
                Alias = null,
                IsReferenceType = false,
                IsNullableReferenceType = false,
                IsValueType = false,
                IsNullableValueType = false,
                IsOptionalWrapper = false,
            };
        }

        if (type.IsGenericType && string.Equals(type.GetGenericTypeDefinition().FullName, OptionalGenericTypeFullName, StringComparison.Ordinal))
        {
            var optionalValueType = FromType(type.GetGenericArguments()[0]);
            return new DiscoveredTypeReference
            {
                Name = optionalValueType.Name,
                Namespace = optionalValueType.Namespace,
                FullName = optionalValueType.FullName,
                Alias = optionalValueType.Alias,
                IsReferenceType = optionalValueType.IsReferenceType,
                IsNullableReferenceType = optionalValueType.IsReferenceType,
                IsValueType = optionalValueType.IsValueType,
                IsNullableValueType = optionalValueType.IsValueType || optionalValueType.IsNullableValueType,
                IsOptionalWrapper = true,
                IsArray = optionalValueType.IsArray,
                ArrayRank = optionalValueType.ArrayRank,
                ElementType = optionalValueType.ElementType,
                GenericArguments = [.. optionalValueType.GenericArguments],
            };
        }

        var genericArguments = type.IsGenericType
            ? type.GetGenericArguments().Select(FromType).ToList()
            : [];

        return new DiscoveredTypeReference
        {
            Name = GetSimpleTypeName(type),
            Namespace = type.Namespace ?? string.Empty,
            FullName = type.FullName,
            Alias = TypeAliasMap.TryGetValue(type.FullName ?? string.Empty, out var alias) ? alias : null,
            IsReferenceType = !type.IsValueType,
            IsNullableReferenceType = false,
            IsValueType = type.IsValueType,
            IsNullableValueType = false,
            IsOptionalWrapper = false,
            IsArray = false,
            ArrayRank = 0,
            ElementType = null,
            GenericArguments = genericArguments,
        };
    }

    private static string GetSimpleTypeName(Type type)
    {
        var name = type.Name;
        var tickIndex = name.IndexOf('`');
        return tickIndex >= 0 ? name[..tickIndex] : name;
    }

    private static readonly Dictionary<string, string> TypeAliasMap = new(StringComparer.Ordinal)
    {
        [typeof(bool).FullName!] = "bool",
        [typeof(byte).FullName!] = "byte",
        [typeof(char).FullName!] = "char",
        [typeof(decimal).FullName!] = "decimal",
        [typeof(double).FullName!] = "double",
        [typeof(float).FullName!] = "float",
        [typeof(int).FullName!] = "int",
        [typeof(long).FullName!] = "long",
        [typeof(object).FullName!] = "object",
        [typeof(sbyte).FullName!] = "sbyte",
        [typeof(short).FullName!] = "short",
        [typeof(string).FullName!] = "string",
        [typeof(uint).FullName!] = "uint",
        [typeof(ulong).FullName!] = "ulong",
        [typeof(ushort).FullName!] = "ushort",
        [typeof(void).FullName!] = "void",
    };
}