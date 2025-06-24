using SimpleBase;

namespace iota_sdk.model.read.transaction
{
    /// <summary>
    /// A transaction will have a (unique) digest.
    /// </summary>
    public readonly struct TransactionDigest : IEquatable<TransactionDigest>, IComparable<TransactionDigest>
    {
        private readonly Digest _digest;

        /// <summary>
        /// Zero value for TransactionDigest
        /// </summary>
        public static readonly TransactionDigest ZERO = new(Digest.ZERO);

        /// <summary>
        /// Creates a new TransactionDigest with the specified byte array
        /// </summary>
        /// <param name="digest">32 byte array representing the digest</param>
        public TransactionDigest(byte[] digest)
        {
            _digest = new Digest(digest);
        }

        /// <summary>
        /// Creates a new TransactionDigest with the specified Digest
        /// </summary>
        /// <param name="digest">The Digest object</param>
        public TransactionDigest(Digest digest)
        {
            _digest = digest;
        }

        /// <summary>
        /// A digest used to signify the parent transaction was the genesis
        /// </summary>
        public static TransactionDigest GenesisMarker() => ZERO;

        /// <summary>
        /// Generate a random TransactionDigest
        /// </summary>
        public static TransactionDigest Random()
        {
            return new TransactionDigest(Digest.Random());
        }

        /// <summary>
        /// Get the inner byte array
        /// </summary>
        public byte[] Inner() => _digest.Inner();

        /// <summary>
        /// Encode the digest using Base58
        /// </summary>
        public string Base58Encode() => Base58.Bitcoin.Encode(_digest.Inner());

        /// <summary>
        /// Get the next lexicographical digest if possible
        /// </summary>
        public TransactionDigest? NextLexicographical()
        {
            var nextDigest = _digest.NextLexicographical();
            return nextDigest.HasValue ? new TransactionDigest(nextDigest.Value) : null;
        }

        /// <summary>
        /// Converts the digest to a byte array
        /// </summary>
        public byte[] ToByteArray() => _digest.ToByteArray();

        /// <summary>
        /// Creates a TransactionDigest from a byte array
        /// </summary>
        public static TransactionDigest FromByteArray(byte[] bytes)
        {
            if (bytes.Length != 32)
                throw new Exception("Invalid transaction digest length. Expected 32 bytes.");

            return new TransactionDigest(bytes);
        }

        /// <summary>
        /// Tries to parse a TransactionDigest from a Base58 string
        /// </summary>
        public static bool TryParse(string s, out TransactionDigest result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch
            {
                result = ZERO;
                return false;
            }
        }

        /// <summary>
        /// Parse a TransactionDigest from a Base58 string
        /// </summary>
        public static TransactionDigest Parse(string s)
        {
            byte[] buffer = Base58.Bitcoin.Decode(s);
            if (buffer.Length != 32)
                throw new Exception("Invalid digest length. Expected 32 bytes");

            return new TransactionDigest(buffer);
        }

        /// <summary>
        /// Returns a string representation of the digest in Base58 format
        /// </summary>
        public override string ToString() => _digest.ToString();

        /// <summary>
        /// Determines whether this instance and another specified TransactionDigest object have the same value
        /// </summary>
        public override bool Equals(object obj) =>
            obj is TransactionDigest other && Equals(other);

        /// <summary>
        /// Determines whether this instance and another specified TransactionDigest object have the same value
        /// </summary>
        public bool Equals(TransactionDigest other) => _digest.Equals(other._digest);

        /// <summary>
        /// Returns the hash code for this TransactionDigest
        /// </summary>
        public override int GetHashCode() => _digest.GetHashCode();

        /// <summary>
        /// Compares this instance with a specified TransactionDigest object
        /// </summary>
        public int CompareTo(TransactionDigest other) => _digest.CompareTo(other._digest);

        /// <summary>
        /// Determines whether two specified TransactionDigest objects have the same value
        /// </summary>
        public static bool operator ==(TransactionDigest left, TransactionDigest right) =>
            left.Equals(right);

        /// <summary>
        /// Determines whether two specified TransactionDigest objects have different values
        /// </summary>
        public static bool operator !=(TransactionDigest left, TransactionDigest right) =>
            !left.Equals(right);

        /// <summary>
        /// Determines whether one specified TransactionDigest is less than another specified TransactionDigest
        /// </summary>
        public static bool operator <(TransactionDigest left, TransactionDigest right) =>
            left.CompareTo(right) < 0;

        /// <summary>
        /// Determines whether one specified TransactionDigest is less than or equal to another specified TransactionDigest
        /// </summary>
        public static bool operator <=(TransactionDigest left, TransactionDigest right) =>
            left.CompareTo(right) <= 0;

        /// <summary>
        /// Determines whether one specified TransactionDigest is greater than another specified TransactionDigest
        /// </summary>
        public static bool operator >(TransactionDigest left, TransactionDigest right) =>
            left.CompareTo(right) > 0;

        /// <summary>
        /// Determines whether one specified TransactionDigest is greater than or equal to another specified TransactionDigest
        /// </summary>
        public static bool operator >=(TransactionDigest left, TransactionDigest right) =>
            left.CompareTo(right) >= 0;

        /// <summary>
        /// Converts a TransactionDigest to a byte array
        /// </summary>
        public static implicit operator byte[](TransactionDigest digest) => digest.ToByteArray();

        /// <summary>
        /// Creates a TransactionDigest from a byte array
        /// </summary>
        public static explicit operator TransactionDigest(byte[] bytes) => FromByteArray(bytes);
    }
}