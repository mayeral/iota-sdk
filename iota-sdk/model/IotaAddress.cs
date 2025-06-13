using System;
using System.Linq;
using System.Numerics;

namespace iota_sdk.model
{
    /// <summary>
    /// Represents an IOTA address, which can be a 256-bit value represented as a hex string.
    /// </summary>
    public class IotaAddress
    {
        private readonly string _address;

        /// <summary>
        /// Creates a new IOTA address from a string representation.
        /// </summary>
        /// <param name="address">The string representation of the address</param>
        public IotaAddress(string address)
        {
            _address = address;
        }

        /// <summary>
        /// Returns the string representation of the address.
        /// </summary>
        public override string ToString()
        {
            return _address;
        }

        /// <summary>
        /// Returns the canonical string representation of the address with full padding.
        /// </summary>
        public string ToCanonicalString(bool withPrefix = true)
        {
            if (!_address.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                return _address;
            }

            string hexPart = _address.Substring(2).PadLeft(64, '0');
            return withPrefix ? $"0x{hexPart}" : hexPart;
        }

        /// <summary>
        /// Returns a shortened representation of the address for display purposes.
        /// </summary>
        public string ToShortString()
        {
            if (_address.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                // Remove leading zeros
                string hexPart = _address.Substring(2).TrimStart('0');
                return hexPart.Length > 0 ? $"0x{hexPart}" : "0x0";
            }
            return _address;
        }

        /// <summary>
        /// Parses a string as an IotaAddress. Valid formats are:
        /// - A 256-bit number, encoded in decimal or hexadecimal with a leading "0x" prefix
        /// - One of the pre-defined named addresses: "std", "iota", "iota_system", "stardust", or "bridge"
        /// </summary>
        /// <param name="address">The string to parse as an address</param>
        /// <returns>An IotaAddress instance</returns>
        /// <exception cref="ArgumentException">Thrown when the address format is invalid</exception>
        public static IotaAddress Parse(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentException("Address cannot be null or empty");
            }

            // Check if it's a named address
            string resolvedAddress = address.ToLowerInvariant() switch
            {
                "std" => "0x1",
                "iota" => "0x2",
                "iota_system" => "0x3",
                "stardust" => "0x107a",
                "bridge" => "0xb",
                _ => address
            };

            // If it starts with 0x, try to parse as hex
            if (resolvedAddress.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                string hexPart = resolvedAddress.Substring(2);
                
                // Validate hex format
                if (!hexPart.All(c => c >= '0' && c <= '9' || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F'))
                {
                    throw new ArgumentException($"Invalid hexadecimal address: {address}");
                }

                // Pad to 64 hex characters (32 bytes) if needed
                if (hexPart.Length <= 64)
                {
                    hexPart = hexPart.PadLeft(64, '0');
                    return new IotaAddress($"0x{hexPart}");
                }
                else
                {
                    throw new ArgumentException($"Hexadecimal address too long: {address}");
                }
            }
            // Try to parse as decimal
            else if (resolvedAddress.All(c => c >= '0' && c <= '9'))
            {
                try
                {
                    // Parse as big integer and convert to hex
                    BigInteger value = BigInteger.Parse(resolvedAddress);
                    
                    // Check if it's a valid size for an address
                    if (value >= BigInteger.Zero && value < BigInteger.One << 256)
                    {
                        string hex = value.ToString("x").PadLeft(64, '0');
                        return new IotaAddress($"0x{hex}");
                    }
                    else
                    {
                        throw new ArgumentException($"Address value out of range: {address}");
                    }
                }
                catch (FormatException)
                {
                    throw new ArgumentException($"Invalid decimal address: {address}");
                }
            }

            throw new ArgumentException($"Invalid address format: {address}");
        }

        /// <summary>
        /// Tries to parse a string as an IotaAddress.
        /// </summary>
        /// <param name="address">The string to parse</param>
        /// <param name="result">The resulting IotaAddress if parsing succeeds</param>
        /// <returns>True if parsing succeeded, false otherwise</returns>
        public static bool TryParse(string address, out IotaAddress result)
        {
            try
            {
                result = Parse(address);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Checks if two IotaAddress instances are equal.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj is IotaAddress other)
            {
                // Compare canonical representations to handle different formats of the same address
                return string.Equals(
                    ToCanonicalString(true), 
                    other.ToCanonicalString(true), 
                    StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        /// <summary>
        /// Returns a hash code for the address.
        /// </summary>
        public override int GetHashCode()
        {
            return ToCanonicalString(true).GetHashCode(StringComparison.OrdinalIgnoreCase);
        }

        // Implicit conversion to string for convenience
        public static implicit operator string(IotaAddress address) => address._address;
        
        // Implicit conversion from string for convenience
        public static implicit operator IotaAddress(string address) => Parse(address);

        // Equality operators
        public static bool operator ==(IotaAddress left, IotaAddress right)
        {
            if (left is null)
                return right is null;
            return left.Equals(right);
        }

        public static bool operator !=(IotaAddress left, IotaAddress right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Creates a new IOTA address from a string representation.
        /// </summary>
        /// <param name="address">The string to parse as an address</param>
        /// <returns>An IotaAddress instance</returns>
        /// <exception cref="ArgumentException">Thrown when the address format is invalid</exception>
        public static IotaAddress FromString(string address)
        {
            return Parse(address);
        }
    }
}