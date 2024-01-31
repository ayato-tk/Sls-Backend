using System;
using System.Text.Json;

namespace Sales.Core.Extensions;

public static class JsonSerializerExtensions
{
    private static JsonSerializerOptions defaultSerializerSettings = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public static string Serialize<T>(this T @object)
    {
        ArgumentNullException.ThrowIfNull(@object, nameof(@object));
        return System.Text.Json.JsonSerializer.Serialize(@object, defaultSerializerSettings);
    }

    public static T Deserialize<T>(this string json)
    {
        ArgumentNullException.ThrowIfNull(json, nameof(json));
        return System.Text.Json.JsonSerializer.Deserialize<T>(json, defaultSerializerSettings);
    }

}