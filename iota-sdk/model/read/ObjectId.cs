namespace iota_sdk.model.read;

/// <summary>
/// Represents an object identifier in the IOTA system.
/// </summary>
[Serializable]
public class ObjectId : IEquatable<ObjectId>, IComparable<ObjectId>
{
    private readonly string _value;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectId"/> class.
    /// </summary>
    /// <param name="value">The hex string representation of the object ID.</param>
    public ObjectId(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        // Ensure the value is a valid hex string
        if (!value.StartsWith("0x"))
            _value = "0x" + value;
        else
            _value = value;
    }

    /// <summary>
    /// Converts the ObjectId to its string representation.
    /// </summary>
    /// <returns>The string representation of the ObjectId.</returns>
    public override string ToString()
    {
        return _value;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object obj)
    {
        if (obj is ObjectId other)
            return Equals(other);
        return false;
    }

    /// <summary>
    /// Determines whether the specified ObjectId is equal to the current ObjectId.
    /// </summary>
    /// <param name="other">The ObjectId to compare with the current ObjectId.</param>
    /// <returns>true if the specified ObjectId is equal to the current ObjectId; otherwise, false.</returns>
    public bool Equals(ObjectId other)
    {
        if (other is null)
            return false;
        return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance.</returns>
    public override int GetHashCode()
    {
        return _value.GetHashCode(StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Compares the current instance with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared.</returns>
    public int CompareTo(ObjectId other)
    {
        if (other is null)
            return 1;
        return string.Compare(_value, other._value, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(ObjectId left, ObjectId right)
    {
        if (left is null)
            return right is null;
        return left.Equals(right);
    }

    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(ObjectId left, ObjectId right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Less than operator.
    /// </summary>
    public static bool operator <(ObjectId left, ObjectId right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    /// <summary>
    /// Greater than operator.
    /// </summary>
    public static bool operator >(ObjectId left, ObjectId right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    /// <summary>
    /// Less than or equal operator.
    /// </summary>
    public static bool operator <=(ObjectId left, ObjectId right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    /// <summary>
    /// Greater than or equal operator.
    /// </summary>
    public static bool operator >=(ObjectId left, ObjectId right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }
}