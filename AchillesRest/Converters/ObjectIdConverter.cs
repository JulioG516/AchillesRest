using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LiteDB;

namespace AchillesRest.Converters;

public class ObjectIdConverter : JsonConverter<ObjectId>
{
    public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = JsonDocument.ParseValue(ref reader);
        var timestamp = json.RootElement.GetProperty("Timestamp").GetInt32();
        var machine = json.RootElement.GetProperty("Machine").GetInt32();
        var pid = json.RootElement.GetProperty("Pid").GetInt16();
        var increment = json.RootElement.GetProperty("Increment").GetInt32();
        var creationTime = json.RootElement.GetProperty("CreationTime").GetDateTime();

        return new ObjectId(timestamp, machine, pid, increment);
    }

    public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Timestamp", value.Timestamp);
        writer.WriteNumber("Machine", value.Machine);
        writer.WriteNumber("Pid", value.Pid);
        writer.WriteNumber("Increment", value.Increment);
        writer.WriteString("CreationTime", value.CreationTime);
        writer.WriteEndObject();
    }
}