using System.Text.Json;
using System.Text.Json.Serialization;

namespace AlYurr_CrestronNvxSdk.HelperFunctions;

// https://stackoverflow.com/questions/70336610/system-text-json-convert-empty-string-to-int
public class JsonInt32Converter : JsonConverter<int>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(int);
    }
    
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        try
        {
            var value = reader.GetInt32();
            return value;
        }
        catch
        {
            return 0;
        }            
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        
    }
}