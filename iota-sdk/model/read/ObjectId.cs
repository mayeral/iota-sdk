using System.Globalization;
using Newtonsoft.Json;

namespace iota_sdk.model.read;

[JsonConverter(typeof(ObjectIDConverter))]
public class ObjectId : IEquatable<ObjectId>
{
    // The number of bytes in an address
    public const int LENGTH = 32;

    // The underlying byte array representing the address
    private readonly byte[] _bytes;

    // Static instances
    public static readonly ObjectId ZERO = new ObjectId(new byte[LENGTH]);
    public static readonly ObjectId MAX = new ObjectId(Enumerable.Repeat((byte)0xff, LENGTH).ToArray());

    // Constructors
    public ObjectId(byte[] bytes)
    {
        if (bytes == null || bytes.Length != LENGTH)
        {
            throw new ArgumentException($"ObjectId must be {LENGTH} bytes");
        }
        _bytes = (byte[])bytes.Clone();
    }

    // Parse an ObjectId from a hex string (with or without 0x prefix)
    public static ObjectId Parse(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            throw new ArgumentException("Cannot parse empty string");
        }

        // Check if the string has 0x prefix and remove it
        if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            s = s.Substring(2);
        }

        // Pad with leading zeros if needed
        if (s.Length < LENGTH * 2)
        {
            s = s.PadLeft(LENGTH * 2, '0');
        }
        else if (s.Length > LENGTH * 2)
        {
            throw new ArgumentException($"Hex string is too long for ObjectId: {s}");
        }

        try
        {
            byte[] bytes = new byte[LENGTH];
            for (int i = 0; i < LENGTH; i++)
            {
                bytes[i] = byte.Parse(s.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return new ObjectId(bytes);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Failed to parse ObjectId from hex: {s}", ex);
        }
    }

    // Try to parse an ObjectId from a hex string
    public static bool TryParse(string s, out ObjectId result)
    {
        result = null;
        try
        {
            result = Parse(s);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Generate a random ObjectId
    public static ObjectId Random()
    {
        byte[] bytes = new byte[LENGTH];
        new Random().NextBytes(bytes);
        return new ObjectId(bytes);
    }

    // Get the bytes of the ObjectId
    public byte[] ToBytes()
    {
        return (byte[])_bytes.Clone();
    }

    // Convert to hex string with 0x prefix
    public string ToHexString()
    {
        return "0x" + BitConverter.ToString(_bytes).Replace("-", "").ToLowerInvariant();
    }

    // Override ToString to return hex representation
    public override string ToString()
    {
        return ToHexString();
    }

    // Equality methods
    public bool Equals(ObjectId other)
    {
        if (other == null) return false;
        return _bytes.SequenceEqual(other._bytes);
    }

    public override bool Equals(object obj)
    {
        return obj is ObjectId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return BitConverter.ToInt32(_bytes, 0);
    }

    // Equality operators
    public static bool operator ==(ObjectId left, ObjectId right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator !=(ObjectId left, ObjectId right)
    {
        return !(left == right);
    }

    // Implicit conversion from string
    public static implicit operator ObjectId(string s)
    {
        return Parse(s);
    }
}

// Custom JSON converter for ObjectId
public class ObjectIDConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ObjectId);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        var value = reader.Value.ToString();
        return ObjectId.Parse(value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var id = (ObjectId)value;
        writer.WriteValue(id.ToHexString());
    }
}