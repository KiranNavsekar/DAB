// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Azure.DataApiBuilder.Config.Converters;

internal class EntitySourceConverterFactory : JsonConverterFactory
{
    /// <inheritdoc/>
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableTo(typeof(EntitySource));
    }

    /// <inheritdoc/>
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return new EntitySourceConverter();
    }

    private class EntitySourceConverter : JsonConverter<EntitySource>
    {
        public override EntitySource? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new EntitySource(reader.GetString() ?? "", EntityType.Table, new(), Enumerable.Empty<string>().ToArray());
            }

            JsonSerializerOptions innerOptions = new(options);
            innerOptions.Converters.Remove(innerOptions.Converters.First(c => c is EntitySourceConverterFactory));

            EntitySource? source = JsonSerializer.Deserialize<EntitySource>(ref reader, innerOptions);

            if (source?.Parameters is not null)
            {
                return source with { Parameters = source.Parameters.ToDictionary(p => p.Key, p => GetClrValue((JsonElement)p.Value)) };
            }

            return source;
        }

        private static object GetClrValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString() ?? "",
                JsonValueKind.Number => element.GetInt32(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                _ => element.ToString()
            };
        }

        public override void Write(Utf8JsonWriter writer, EntitySource value, JsonSerializerOptions options)
        {
            JsonSerializerOptions innerOptions = new(options);
            innerOptions.Converters.Remove(innerOptions.Converters.First(c => c is EntitySourceConverterFactory));

            JsonSerializer.Serialize(writer, value, innerOptions);
        }
    }
}