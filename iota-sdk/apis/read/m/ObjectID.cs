using System;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

[JsonConverter(typeof(ObjectIDConverter))]
public class ObjectID : IEquatable<ObjectID>
{
    // The number of bytes in an address
    public const int LENGTH = 32;

    // The underlying byte array representing the address
    private readonly byte[] _bytes;

    // Static instances
    public static readonly ObjectID ZERO = new ObjectID(new byte[LENGTH]);
    public static readonly ObjectID MAX = new ObjectID(Enumerable.Repeat((byte)0xff, LENGTH).ToArray());

    // Constructors
    public ObjectID(byte[] bytes)
    {
        if (bytes == null || bytes.Length != LENGTH)
        {
            throw new ArgumentException($"ObjectID must be {LENGTH} bytes");
        }
        _bytes = (byte[])bytes.Clone();
    }

    // Parse an ObjectID from a hex string (with or without 0x prefix)
    public static ObjectID Parse(string s)
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
            throw new ArgumentException($"Hex string is too long for ObjectID: {s}");
        }

        try
        {
            byte[] bytes = new byte[LENGTH];
            for (int i = 0; i < LENGTH; i++)
            {
                bytes[i] = byte.Parse(s.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return new ObjectID(bytes);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Failed to parse ObjectID from hex: {s}", ex);
        }
    }

    // Try to parse an ObjectID from a hex string
    public static bool TryParse(string s, out ObjectID result)
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

    // Generate a random ObjectID
    public static ObjectID Random()
    {
        byte[] bytes = new byte[LENGTH];
        new Random().NextBytes(bytes);
        return new ObjectID(bytes);
    }

    // Get the bytes of the ObjectID
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
    public bool Equals(ObjectID other)
    {
        if (other == null) return false;
        return _bytes.SequenceEqual(other._bytes);
    }

    public override bool Equals(object obj)
    {
        return obj is ObjectID other && Equals(other);
    }

    public override int GetHashCode()
    {
        return BitConverter.ToInt32(_bytes, 0);
    }

    // Equality operators
    public static bool operator ==(ObjectID left, ObjectID right)
    {
        if (ReferenceEquals(left, null))
            return ReferenceEquals(right, null);
        return left.Equals(right);
    }

    public static bool operator !=(ObjectID left, ObjectID right)
    {
        return !(left == right);
    }

    // Implicit conversion from string
    public static implicit operator ObjectID(string s)
    {
        return Parse(s);
    }
}

// Custom JSON converter for ObjectID
public class ObjectIDConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ObjectID);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;

        var value = reader.Value.ToString();
        return ObjectID.Parse(value);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var id = (ObjectID)value;
        writer.WriteValue(id.ToHexString());
    }
}