using System.Globalization;
using System.Reflection;

namespace Alethic.Auth0.Operator.ModelGenerator.Discovery;

internal static class AttributeValueFormatter
{
    public static string Format(CustomAttributeTypedArgument argument)
    {
        if (argument.Value is null)
        {
            return "null";
        }

        var argumentType = argument.ArgumentType;
        var argumentTypeFullName = argumentType.FullName;
        if (string.Equals(argumentTypeFullName, typeof(string).FullName, StringComparison.Ordinal))
        {
            return "\"" + ((string)argument.Value).Replace("\\", "\\\\", StringComparison.Ordinal).Replace("\"", "\\\"", StringComparison.Ordinal) + "\"";
        }

        if (string.Equals(argumentTypeFullName, typeof(char).FullName, StringComparison.Ordinal))
        {
            return "'" + (((char)argument.Value).ToString()).Replace("\\", "\\\\", StringComparison.Ordinal).Replace("'", "\\'", StringComparison.Ordinal) + "'";
        }

        if (string.Equals(argumentTypeFullName, typeof(bool).FullName, StringComparison.Ordinal))
        {
            return ((bool)argument.Value) ? "true" : "false";
        }

        if (string.Equals(argumentTypeFullName, typeof(float).FullName, StringComparison.Ordinal))
        {
            return ((float)argument.Value).ToString(CultureInfo.InvariantCulture) + "F";
        }

        if (string.Equals(argumentTypeFullName, typeof(double).FullName, StringComparison.Ordinal))
        {
            return ((double)argument.Value).ToString("R", CultureInfo.InvariantCulture) + "D";
        }

        if (string.Equals(argumentTypeFullName, typeof(decimal).FullName, StringComparison.Ordinal))
        {
            return ((decimal)argument.Value).ToString(CultureInfo.InvariantCulture) + "M";
        }

        if (string.Equals(argumentTypeFullName, typeof(long).FullName, StringComparison.Ordinal))
        {
            return ((long)argument.Value).ToString(CultureInfo.InvariantCulture) + "L";
        }

        if (string.Equals(argumentTypeFullName, typeof(uint).FullName, StringComparison.Ordinal))
        {
            return ((uint)argument.Value).ToString(CultureInfo.InvariantCulture) + "U";
        }

        if (string.Equals(argumentTypeFullName, typeof(ulong).FullName, StringComparison.Ordinal))
        {
            return ((ulong)argument.Value).ToString(CultureInfo.InvariantCulture) + "UL";
        }

        if (string.Equals(argumentTypeFullName, typeof(Type).FullName, StringComparison.Ordinal))
        {
            return $"typeof({((Type)argument.Value).FullName})";
        }

        if (argumentType.IsEnum)
        {
            var enumName = Enum.GetName(argumentType, argument.Value);
            return enumName is null
                ? Convert.ToString(argument.Value, CultureInfo.InvariantCulture) ?? "0"
                : $"{argumentType.FullName}.{enumName}";
        }

        if (argument.Value is IReadOnlyCollection<CustomAttributeTypedArgument> collection)
        {
            var values = string.Join(", ", collection.Select(Format));
            return $"new[] {{ {values} }}";
        }

        return Convert.ToString(argument.Value, CultureInfo.InvariantCulture) ?? "null";
    }
}