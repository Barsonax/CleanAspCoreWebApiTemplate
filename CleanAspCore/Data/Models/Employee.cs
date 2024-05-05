using System.Text.Json;
using System.Text.Json.Serialization;

namespace CleanAspCore.Data.Models;

public class Employee
{
    public required EmployeeId Id { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required EmailAddress Email { get; set; }
    public required string Gender { get; set; }
    public virtual Department? Department { get; init; }
    public required Guid DepartmentId { get; set; }
    public virtual Job? Job { get; init; }
    public required Guid JobId { get; set; }
}

public readonly record struct EmployeeId(Guid Value)
{
    public static EmployeeId CreateNew() => new(Guid.NewGuid());

    public static bool TryParse(string value, out EmployeeId id) => StrongIdHelper.Deserialize(value, out id);

    public override string ToString() => StrongIdHelper.Serialize(Value);
}

public class StrongIdJsonConverter<TId> : JsonConverter<TId> where TId : struct
{
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        StrongIdHelper.Deserialize(reader.GetString()!, out TId employeeId);
        return employeeId;
    }

    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(StrongIdHelper.Serialize(value));
    }
}

public static class StrongIdHelper
{
    public static string Serialize<TValue>(TValue value)
        => value?.ToString() ?? string.Empty;

    public static bool Deserialize<TId>(string id, out TId parsedId)
        where TId : struct
    {
        if (Guid.TryParse(id, out var guid))
        {
            parsedId = (TId)(Activator.CreateInstance(typeof(TId), guid) ?? throw new InvalidOperationException());
            return true;
        }
        else
        {
            parsedId = new TId();
            return false;
        }
    }
}
