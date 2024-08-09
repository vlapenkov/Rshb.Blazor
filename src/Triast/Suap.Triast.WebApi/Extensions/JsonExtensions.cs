using System.Text.Json;
using System.Text.Json.Nodes;

namespace Suap.Triast.WebApi.Extensions;

public static class JsonExtensions
{
    public static T? GetValue<T>(this JsonObject jsonObject, string jsonPath)
    {
        var result = jsonObject.SelectJsonNode(jsonPath);

        if (result == default)
        {
            return default;
        }

        return result.Node switch
        {
            JsonObject obj => obj.TryGetPropertyValue(result.PropertyName, out var property)
                ? property!.GetValue<T>()
                : default,
            JsonArray array => array.Deserialize<T>(),
            _ => result.Node!.GetValue<T>()
        };
    }

    private static (JsonNode? Node, string PropertyName) SelectJsonNode(this JsonObject jsonObject, string jsonPath)
    {
        if (jsonPath.Contains('.'))
        {
            var @object = jsonObject;
            foreach (var prop in new SpanSplitter(jsonPath.AsSpan(), '.'))
            {
                var propertyName = prop.ToString();
                if (@object.TryGetPropertyValue(propertyName, out var node))
                {
                    switch (node)
                    {
                        case JsonObject jsonNode:
                            @object = jsonNode;
                            break;
                        case JsonValue:
                            return (node.Parent!, propertyName);
                        default:
                            return (node, propertyName);
                    }
                }
                else
                {
                    return default;
                }
            }
        }

        if (jsonObject.TryGetPropertyValue(jsonPath, out var n))
        {
            return n switch
            {
                JsonValue => (n.Parent!, propertyPath: jsonPath),
                _ => (n, propertyPath: jsonPath)
            };
        }

        return default;
    }
}
