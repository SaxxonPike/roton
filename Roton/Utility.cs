using System;
using System.IO;
using System.Text;

namespace Roton
{
    public static class Utility
    {
        private static Encoding _codePage437 = Encoding.GetEncoding(437);
        private static string _hexAlphabet = "0123456789ABCDEF";

        /// <summary>
        /// Return the absolute difference between this value and another specified value.
        /// </summary>
        internal static int AbsDiff(this int a, int b)
        {
            var diff = a - b;
            if (diff < 0)
                return -diff;
            return diff;
        }

        /// <summary>
        /// Return 1 if the value is above zero, -1 if the value is below zero, and 0 otherwise.
        /// </summary>
        internal static int Polarity(this int value)
        {
            if (value > 0)
                return 1;
            else if (value < 0)
                return -1;
            return 0;
        }

        /// <summary>
        /// Read a pascal-style string using code page 437.
        /// </summary>
        internal static string ReadPascalString(this BinaryReader reader)
        {
            int length = reader.ReadByte();
            var data = reader.ReadBytes(length);
            return _codePage437.GetString(data);
        }

        /// <summary>
        /// Read a pascal-style string using code page 437, assuming padding.
        /// </summary>
        internal static string ReadPascalString(this BinaryReader reader, int maxLength)
        {
            var data = reader.ReadBytes(maxLength + 1);
            int length = data[0];
            var result = new byte[length];
            Array.Copy(data, 1, result, 0, length);
            return _codePage437.GetString(result);
        }

        /// <summary>
        /// Return the squared result of an integer.
        /// </summary>
        internal static int Square(this int i)
        {
            return i*i;
        }

        /// <summary>
        /// Convert an ASCII value to a one-character string.
        /// </summary>
        internal static string ToAscii(this int value)
        {
            return _codePage437.GetString(new byte[] {(byte) (value & 0xFF)});
        }

        /// <summary>
        /// Convert a string to a byte array using code page 437.
        /// </summary>
        internal static byte[] ToBytes(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new byte[0];
            }
            return _codePage437.GetBytes(value);
        }

        /// <summary>
        /// Convert an integer to a character using code page 437.
        /// </summary>
        internal static char ToChar(this int value)
        {
            return _codePage437.GetChars(new byte[] {(byte) (value & 0xFF)})[0];
        }

        /// <summary>
        /// Convert a value to a hex string.
        /// </summary>
        private static string ToHex(int value, int chars)
        {
            var result = "";
            while (chars > 0)
            {
                result = _hexAlphabet[value & 0xF] + result;
                chars--;
                value >>= 4;
            }
            return "0x" + result;
        }

        /// <summary>
        /// Convert a value to an 8-bit hex string.
        /// </summary>
        public static string ToHex8(this int value)
        {
            return ToHex(value, 2);
        }

        /// <summary>
        /// Convert a value to a 16-bit hex string.
        /// </summary>
        public static string ToHex16(this int value)
        {
            return ToHex(value, 4);
        }

        /// <summary>
        /// Convert a value to a 32-bit hex string.
        /// </summary>
        public static string ToHex32(this int value)
        {
            return ToHex(value, 8);
        }

        /// <summary>
        /// Get the lowercase representation of an ASCII char stored as a byte.
        /// </summary>
        internal static int ToLowerCase(this byte value)
        {
            if (value >= 0x41 && value <= 0x5A)
            {
                value -= 0x20;
            }
            return value;
        }

        /// <summary>
        /// Get the lowercase representation of a string using code page 437.
        /// </summary>
        internal static byte[] ToLowerCase(this byte[] value)
        {
            var length = value.Length;
            var result = new byte[length];
            value.CopyTo(result, 0);
            for (var i = 0; i < length; i++)
            {
                if (result[i] >= 0x41 && result[i] <= 0x5A)
                {
                    result[i] += 0x20;
                }
            }
            return result;
        }

        /// <summary>
        /// Get the lowercase representation of an ASCII char stored as an integer.
        /// </summary>
        internal static int ToLowerCase(this int value)
        {
            if (value >= 0x41 && value <= 0x5A)
            {
                value -= 0x20;
            }
            return value;
        }

        /// <summary>
        /// Convert an integer to a string using code page 437.
        /// </summary>
        internal static string ToStringValue(this int value)
        {
            return _codePage437.GetString(new byte[] {(byte) (value & 0xFF)});
        }

        /// <summary>
        /// Convert a byte array to a string using code page 437.
        /// </summary>
        internal static string ToStringValue(this byte[] value)
        {
            return _codePage437.GetString(value);
        }

        /// <summary>
        /// Get the uppercase representation of an ASCII char stored as a byte.
        /// </summary>
        internal static int ToUpperCase(this byte value)
        {
            if (value >= 0x61 && value <= 0x7A)
            {
                value -= 0x20;
            }
            return value;
        }

        /// <summary>
        /// Get the uppercase representation of a string using code page 437.
        /// </summary>
        internal static byte[] ToUpperCase(this byte[] value)
        {
            var length = value.Length;
            var result = new byte[length];
            value.CopyTo(result, 0);
            for (var i = 0; i < length; i++)
            {
                if (result[i] >= 0x61 && result[i] <= 0x7A)
                {
                    result[i] -= 0x20;
                }
            }
            return result;
        }

        /// <summary>
        /// Get the uppercase representation of an ASCII char stored as an integer.
        /// </summary>
        internal static int ToUpperCase(this int value)
        {
            if (value >= 0x61 && value <= 0x7A)
            {
                value -= 0x20;
            }
            return value;
        }

        /// <summary>
        /// Write a pascal-style string using code page 437.
        /// </summary>
        internal static void WritePascalString(this BinaryWriter writer, string value)
        {
            var length = (byte) (value.Length & 0xFF);
            var buffer = new byte[length + 1];
            byte[] data = value.ToBytes();
            Array.Copy(data, 0, buffer, 1, length);
            buffer[0] = length;
            writer.Write(buffer);
        }
    }
}