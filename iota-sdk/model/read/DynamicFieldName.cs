using Newtonsoft.Json;

/// <summary>
/// Represents a dynamic field name with a type and a value.
/// </summary>
public class DynamicFieldName
{
    /// <summary>
    /// Gets or sets the type of the dynamic field.
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the dynamic field.
    /// </summary>
    [JsonProperty("value")]
    public object? Value { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicFieldName"/> class.
    /// </summary>  
    public DynamicFieldName()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicFieldName"/> class with specified type and value.
    /// </summary>
    /// <param name="type">The type of the dynamic field.</param>
    /// <param name="value">The value of the dynamic field.</param>
    public DynamicFieldName(string type, object? value)
    {
        Type = type;
        Value = value;
    }

    /// <summary>
    /// Returns a string representation of the dynamic field name.
    /// </summary>
    /// <returns>A string representation of the dynamic field name.</returns>
    public override string ToString()
    {
        return $"{{ \"type\": \"{Type}\", \"value\": {JsonConvert.SerializeObject(Value)} }}";
    }
}