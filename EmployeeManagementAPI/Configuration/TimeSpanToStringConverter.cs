using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmployeeManagementAPI.Configurations
{
    public class TimeSpanToStringConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString() ?? string.Empty;
            return TimeSpan.Parse(str);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            // Serialize TimeSpan as "hh:mm:ss"
            writer.WriteStringValue(value.ToString(@"hh\:mm\:ss"));
        }
    }
}
