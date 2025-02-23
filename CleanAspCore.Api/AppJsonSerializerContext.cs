using System.Text.Json.Serialization;

namespace CleanAspCore.Api;

[JsonSerializable(typeof(ProblemDetails))]
[JsonSourceGenerationOptions(
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    WriteIndented = true)]
public partial class AppJsonSerializerContext : JsonSerializerContext;
