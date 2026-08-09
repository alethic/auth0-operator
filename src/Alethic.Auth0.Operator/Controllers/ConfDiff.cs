using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Alethic.Auth0.Operator.Controllers
{

    /// <summary>
    /// Structural comparison of two instances of the same conf model, used to decide whether a remote entity
    /// actually requires an update. Comparison happens between our own conf models (the spec conf and the conf
    /// read back from Auth0 by <c>Get</c>/<c>FromApi</c>) — never between SDK request/response types.
    /// </summary>
    static class ConfDiff
    {

        static readonly JsonSerializerOptions SerializerOptions = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        /// <summary>
        /// Returns <c>true</c> when every property set on <paramref name="desired"/> has an equal value on
        /// <paramref name="actual"/>. Properties that are null on <paramref name="desired"/> are unmanaged by the
        /// spec and are ignored, as are properties present only on <paramref name="actual"/> (Auth0-side defaults
        /// the spec does not manage). Top-level properties named in <paramref name="excludedProperties"/> (JSON
        /// property names) are removed from the comparison; use this for create-only or separately-reconciled
        /// fields that an update can never change.
        /// </summary>
        /// <param name="desired"></param>
        /// <param name="actual"></param>
        /// <param name="excludedProperties"></param>
        public static bool IsSubsetOf(object? desired, object? actual, params string[] excludedProperties)
        {
            if (desired is null)
                return true;
            if (actual is null)
                return false;

            var desiredNode = JsonSerializer.SerializeToNode(desired, SerializerOptions);
            var actualNode = JsonSerializer.SerializeToNode(actual, SerializerOptions);

            if (desiredNode is JsonObject desiredObject)
                foreach (var property in excludedProperties)
                    desiredObject.Remove(property);

            return IsSubsetOf(desiredNode, actualNode);
        }

        /// <summary>
        /// Returns <c>true</c> when <paramref name="desired"/> and <paramref name="actual"/> are structurally
        /// identical. Unlike <see cref="IsSubsetOf(object?, object?, string[])"/> this is symmetric; use it for
        /// full-replace updates where a property removed from the spec must still be pushed.
        /// </summary>
        /// <param name="desired"></param>
        /// <param name="actual"></param>
        public static bool DeepEquals(object? desired, object? actual)
        {
            return JsonNode.DeepEquals(
                JsonSerializer.SerializeToNode(desired, SerializerOptions),
                JsonSerializer.SerializeToNode(actual, SerializerOptions));
        }

        static bool IsSubsetOf(JsonNode? desired, JsonNode? actual)
        {
            // a null desired value means the property is unmanaged
            if (desired is null)
                return true;
            if (actual is null)
                return false;

            if (desired is JsonObject desiredObject)
            {
                if (actual is not JsonObject actualObject)
                    return false;

                foreach (var property in desiredObject)
                {
                    if (property.Value is null)
                        continue;
                    if (actualObject.TryGetPropertyValue(property.Key, out var actualValue) == false)
                        return false;
                    if (IsSubsetOf(property.Value, actualValue) == false)
                        return false;
                }

                return true;
            }

            if (desired is JsonArray desiredArray)
            {
                if (actual is not JsonArray actualArray)
                    return false;
                if (desiredArray.Count != actualArray.Count)
                    return false;

                for (var i = 0; i < desiredArray.Count; i++)
                    if (IsSubsetOf(desiredArray[i], actualArray[i]) == false)
                        return false;

                return true;
            }

            return JsonNode.DeepEquals(desired, actual);
        }

    }

}
