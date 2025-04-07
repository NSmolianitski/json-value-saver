using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Dto;

namespace Application.Converters;

public class ReplaceValuesRequestDtoConverter : JsonConverter<ReplaceValuesRequestDto>
{
    public override ReplaceValuesRequestDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var values = new List<SaveValueDto>();

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Ожидался массив");

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Ожидался объект");

            reader.Read();

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException("Ожидалось имя свойства");

            var key = reader.GetString();
            if (!int.TryParse(key, out var code))
                throw new JsonException("Ключ должен быть числом");

            reader.Read();
            var value = reader.GetString();

            reader.Read();
            if (reader.TokenType != JsonTokenType.EndObject)
                throw new JsonException("Ожидался конец объекта");

            values.Add(new SaveValueDto(code, value));
        }

        return new ReplaceValuesRequestDto(values);
    }

    public override void Write(Utf8JsonWriter writer, ReplaceValuesRequestDto value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var item in value.Values)
        {
            writer.WriteStartObject();
            writer.WriteString(item.Code.ToString(), item.Value);
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
    }
}
