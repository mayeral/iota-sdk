using SimpleBase;

namespace iota_sdk.model.read
{
    /// <summary>
    /// Represents a 32-byte digest value
    /// </summary>
    public readonly struct Digest : IEquatable<Digest>, IComparable<Digest>
    {
        private readonly byte[] _bytes;

        /// <summary>
        /// Zero value for Digest
        /// </summary>
        public static readonly Digest ZERO = new Digest(new byte[32]);

        /// <summary>
        /// Creates a new Digest with the specified byte array
        /// </summary>
        /// <param name="bytes">32 byte array</param>
        /// <exception cref="Exception">Thrown when the byte array length is not 32</exception>
        public Digest(byte[] bytes)
        {
            if (bytes.Length != 32)
                throw new Exception($"Invalid digest length. Expected 32 bytes, got {bytes.Length}");

            _bytes = new byte[32];
            Array.Copy(bytes, _bytes, 32);
        }

        /// <summary>
        /// Generate a random Digest
        /// </summary>
        public static Digest Random()
        {
            var bytes = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return new Digest(bytes);
        }

        /// <summary>
        /// Get the inner byte array
        /// </summary>
        public byte[] Inner()
        {
            var result = new byte[32];
            Array.Copy(_bytes, result, 32);
            return result;
        }

        /// <summary>
        /// Convert to byte array
        /// </summary>
        public byte[] ToByteArray() => Inner();

        /// <summary>
        /// Get the next lexicographical digest if possible
        /// </summary>
        public Digest? NextLexicographical()
        {
            // Find the rightmost byte that isn't 255
            int pos = -1;
            for (int i = 31; i >= 0; i--)
            {
                if (_bytes[i] != 255)
                {
                    pos = i;
                    break;
                }
            }

            // If all bytes are 255, there is no next lexicographical value
            if (pos == -1)
                return null;

            var nextBytes = Inner();
            nextBytes[pos]++;
            
            // Set all bytes to the right to 0
            for (int i = pos + 1; i < 32; i++)
            {
                nextBytes[i] = 0;
            }

            return new Digest(nextBytes);
        }

        /// <summary>
        /// Returns a string representation of the digest in Base58 format
        /// </summary>
        public override string ToString() => Base58.Bitcoin.Encode(_bytes);

        /// <summary>
        /// Determines whether this instance and another specified Digest object have the same value
        /// </summary>
        public override bool Equals(object obj) => 
            obj is Digest other && Equals(other);

        /// <summary>
        /// Determines whether this instance and another specified Digest object have the same value
        /// </summary>
        public bool Equals(Digest other)
        {
            if (_bytes == null)
                return other._bytes == null;

            if (other._bytes == null)
                return false;

            return _bytes.SequenceEqual(other._bytes);
        }

        /// <summary>
        /// Returns the hash code for this Digest
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (byte b in _bytes)
                {
                    hash = hash * 31 + b;
                }
                return hash;
            }
        }

        /// <summary>
        /// Compares this instance with a specified Digest object
        /// </summary>
        public int CompareTo(Digest other)
        {
            for (int i = 0; i < 32; i++)
            {
                int comparison = _bytes[i].CompareTo(other._bytes[i]);
                if (comparison != 0)
                    return comparison;
            }
            return 0;
        }

        /// <summary>
        /// Determines whether two specified Digest objects have the same value
        /// </summary>
        public static bool operator ==(Digest left, Digest right) => 
            left.Equals(right);

        /// <summary>
        /// Determines whether two specified Digest objects have different values
        /// </summary>
        public static bool operator !=(Digest left, Digest right) => 
            !left.Equals(right);

        /// <summary>
        /// Determines whether one specified Digest is less than another specified Digest
        /// </summary>
        public static bool operator <(Digest left, Digest right) => 
            left.CompareTo(right) < 0;

        /// <summary>
        /// Determines whether one specified Digest is less than or equal to another specified Digest
        /// </summary>
        public static bool operator <=(Digest left, Digest right) => 
            left.CompareTo(right) <= 0;

        /// <summary>
        /// Determines whether one specified Digest is greater than another specified Digest
        /// </summary>
        public static bool operator >(Digest left, Digest right) => 
            left.CompareTo(right) > 0;

        /// <summary>
        /// Determines whether one specified Digest is greater than or equal to another specified Digest
        /// </summary>
        public static bool operator >=(Digest left, Digest right) => 
            left.CompareTo(right) >= 0;

        /// <summary>
        /// Converts a Digest to a byte array
        /// </summary>
        public static implicit operator byte[](Digest digest) => digest.ToByteArray();

        /// <summary>
        /// Creates a Digest from a byte array
        /// </summary>
        public static explicit operator Digest(byte[] bytes) => new Digest(bytes);
    }
}