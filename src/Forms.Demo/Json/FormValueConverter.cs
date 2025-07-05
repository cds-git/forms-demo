using System.Text.Json;
using System.Text.Json.Serialization;

using Forms.Demo.Forms;

namespace Forms.Demo.Json;

public class FormValueConverter : JsonConverter<FormValue>
{
    public override FormValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => new TextValue(reader.GetString()!),
            JsonTokenType.Number => ReadNumber(ref reader),
            JsonTokenType.True => new BooleanValue(true),
            JsonTokenType.False => new BooleanValue(false),
            JsonTokenType.Null => new NullValue(),
            JsonTokenType.StartArray => ReadArray(ref reader, options),
            JsonTokenType.StartObject => ReadObject(ref reader, options),
            _ => throw new JsonException($"Unsupported token type: {reader.TokenType}")
        };
    }

    private FormValue ReadNumber(ref Utf8JsonReader reader)
    {
        // Try to determine the best numeric type to preserve precision and range
        // Start with smallest appropriate type for memory efficiency
        if (reader.TryGetInt32(out int intValue))
        {
            return new IntValue(intValue);
        }
        else if (reader.TryGetInt64(out long longValue))
        {
            return new LongValue(longValue);
        }
        else if (reader.TryGetDecimal(out decimal decimalValue))
        {
            return new DecimalValue(decimalValue);
        }
        else if (reader.TryGetDouble(out double doubleValue))
        {
            return new FloatValue(doubleValue);
        }
        else
        {
            throw new JsonException("Unable to parse number");
        }
    }

    private ArrayValue ReadArray(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var list = new List<FormValue>();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            FormValue value = Read(ref reader, typeof(FormValue), options);
            list.Add(value);
        }

        return new ArrayValue(list.ToArray());
    }

    private ObjectValue ReadObject(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        var dict = new Dictionary<string, FormValue>();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException("Expected PropertyName token");

            string key = reader.GetString()!;
            reader.Read();

            FormValue value = Read(ref reader, typeof(FormValue), options);
            dict[key] = value;
        }

        return new ObjectValue(dict);
    }

    public override void Write(Utf8JsonWriter writer, FormValue value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case TextValue text:
                writer.WriteStringValue(text.Value);
                break;
            case IntValue intVal:
                writer.WriteNumberValue(intVal.Value);
                break;
            case LongValue longVal:
                writer.WriteNumberValue(longVal.Value);
                break;
            case DecimalValue decimalVal:
                writer.WriteNumberValue(decimalVal.Value);
                break;
            case FloatValue floatVal:
                writer.WriteNumberValue(floatVal.Value);
                break;
            case BooleanValue boolean:
                writer.WriteBooleanValue(boolean.Value);
                break;
            case DateValue date:
                writer.WriteStringValue(date.Value.ToString("O"));
                break;
            case ArrayValue array:
                writer.WriteStartArray();
                foreach (var item in array.Value)
                {
                    Write(writer, item, options);
                }
                writer.WriteEndArray();
                break;
            case ObjectValue obj:
                writer.WriteStartObject();
                foreach (var kvp in obj.Value)
                {
                    writer.WritePropertyName(kvp.Key);
                    Write(writer, kvp.Value, options);
                }
                writer.WriteEndObject();
                break;
            case NullValue:
                writer.WriteNullValue();
                break;
            default:
                throw new JsonException($"Unsupported FormValue type: {value.GetType()}");
        }
    }
}