using System;
using System.Text.Json;

namespace SqlSherlock.Data.Tests
{
    internal static class JsonElementHelper
    {
        public static JsonElement FromPrimitive(object value)
        {
            string json = value switch
            {
                string s => $"\"{s}\"",
                int or long or double or float or bool or decimal => Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture),
                _ => throw new ArgumentException("Unsupported type. Only string, numeric, and boolean types are supported.", nameof(value)),
            };
            return JsonDocument.Parse(json).RootElement.Clone();
        }
    }
}
