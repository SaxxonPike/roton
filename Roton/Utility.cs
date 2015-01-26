using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Roton
{
    static public class Utility
    {
        static private Encoding CodePage437 = Encoding.GetEncoding(437);
        static private string HexAlphabet = "0123456789ABCDEF";

        /// <summary>
        /// Return the absolute difference between this value and another specified value.
        /// </summary>
        static internal int AbsDiff(this int a, int b)
        {
            int diff = (a - b);
            if (diff < 0)
                return -diff;
            return diff;
        }

        /// <summary>
        /// Return 1 if the value is above zero, -1 if the value is below zero, and 0 otherwise.
        /// </summary>
        static internal int Polarity(this int value)
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
        static internal string ReadPascalString(this BinaryReader reader)
        {
            int length = reader.ReadByte();
            byte[] data = reader.ReadBytes(length);
            return CodePage437.GetString(data);
        }

        /// <summary>
        /// Read a pascal-style string using code page 437, assuming padding.
        /// </summary>
        static internal string ReadPascalString(this BinaryReader reader, int maxLength)
        {
            byte[] data = reader.ReadBytes(maxLength + 1);
            int length = data[0];
            byte[] result = new byte[length];
            Array.Copy(data, 1, result, 0, length);
            return CodePage437.GetString(result);
        }

        /// <summary>
        /// Return the squared result of an integer.
        /// </summary>
        static internal int Square(this int i)
        {
            return i * i;
        }

        /// <summary>
        /// Convert an ASCII value to a one-character string.
        /// </summary>
        static internal string ToAscii(this int value)
        {
            return CodePage437.GetString(new byte[] { (byte)(value & 0xFF) });
        }

        /// <summary>
        /// Convert a string to a byte array using code page 437.
        /// </summary>
        static internal byte[] ToBytes(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new byte[0];
            }
            return CodePage437.GetBytes(value);
        }

        /// <summary>
        /// Convert a value to a hex string.
        /// </summary>
        static private string ToHex(int value, int chars)
        {
            string result = "";
            while (chars > 0)
            {
                result = HexAlphabet[value & 0xF] + result;
                chars--;
                value >>= 4;
            }
            return "0x" + result;
        }

        /// <summary>
        /// Convert a value to an 8-bit hex string.
        /// </summary>
        static public string ToHex8(this int value)
        {
            return ToHex(value, 2);
        }

        /// <summary>
        /// Convert a value to a 16-bit hex string.
        /// </summary>
        static public string ToHex16(this int value)
        {
            return ToHex(value, 4);
        }

        /// <summary>
        /// Convert a value to a 32-bit hex string.
        /// </summary>
        static public string ToHex32(this int value)
        {
            return ToHex(value, 8);
        }

        /// <summary>
        /// Get the lowercase representation of a string using code page 437.
        /// </summary>
        static internal byte[] ToLowerCase(this byte[] value)
        {
            int length = value.Length;
            byte[] result = new byte[length];
            value.CopyTo(result, 0);
            for (int i = 0; i < length; i++)
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
        static internal int ToLowerCase(this int value)
        {
            if (value >= 0x41 && value <= 0x5A)
            {
                value -= 0x20;
            }
            return value;
        }

        /// <summary>
        /// Convert a byte array to a string using code page 437.
        /// </summary>
        static internal string ToStringValue(this byte[] value)
        {
            return CodePage437.GetString(value);
        }

        /// <summary>
        /// Get the uppercase representation of a string using code page 437.
        /// </summary>
        static internal byte[] ToUpperCase(this byte[] value)
        {
            int length = value.Length;
            byte[] result = new byte[length];
            value.CopyTo(result, 0);
            for (int i = 0; i < length; i++)
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
        static internal int ToUpperCase(this int value)
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
        static internal void WritePascalString(this BinaryWriter writer, string value)
        {
            byte length = (byte)(value.Length & 0xFF);
            byte[] buffer = new byte[length + 1];
            byte[] data = value.ToBytes();
            Array.Copy(data, 0, buffer, 1, length);
            buffer[0] = length;
            writer.Write(buffer);
        }
    }
}
