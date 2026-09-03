using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Infrastructure;

internal static class Utility
{
    extension(int a)
    {
        public ReadOnlySpan<char> ToCharSpan(Span<char> buffer)
        {
            var idx = buffer.Length;
            var val = a;

            if (a == 0)
            {
                buffer[--idx] = '0';
                return buffer.Slice(idx);
            }
            
            var neg = val < 0;
            if (neg)
                val = -val;

            while (val > 0)
            {
                var num = val % 10;
                buffer[--idx] = (char)('0' + num);
                val /= 10;
            }
            
            if (neg)
                buffer[--idx] = '-';

            return buffer.Slice(idx);
        }
        
        /// <summary>
        /// Return the absolute difference between this value and another specified value.
        /// </summary>
        public int AbsDiff(int b) =>
            Math.Abs(a - b);

        /// <summary>
        /// Return 1 if the value is above zero, -1 if the value is below zero, and 0 otherwise.
        /// </summary>
        public int Polarity() =>
            Math.Sign(a);

        /// <summary>
        /// Return the squared result of an integer.
        /// </summary>
        public int Square() =>
            a * a;

        /// <summary>
        /// Convert an integer to a character using code page 437.
        /// </summary>
        public char ToChar() => 
            Cp437.ByteToChar(unchecked((byte)a));

        /// <summary>
        /// Get the uppercase representation of an ASCII char stored as an integer.
        /// </summary>
        private int ToUpperCase() =>
            a switch
            {
                >= 0x61 and <= 0x7A => a - 0x20,
                _ => a
            };
    }

    /// <summary>
    /// Convert a byte array to a string using code page 437.
    /// </summary>
    public static string ToStringValue(this byte[] value)
    {
#if NET10_0_OR_GREATER
        return string.Create(value.Length, value, static (span, bytes) => Cp437.BytesToChars(bytes, span));
#else
        var str = new char[value.Length];
        Cp437.BytesToChars(value, str);
        return new string(str);
#endif
    }

    /// <summary>
    /// Get the uppercase representation of an input key.
    /// </summary>
    public static EngineKeyCode ToUpperCase(this EngineKeyCode value) =>
        (EngineKeyCode)((int)value).ToUpperCase();

    /// <summary>
    /// Get the uppercase representation of an ASCII char stored as a byte.
    /// </summary>
    public static int ToUpperCase(this byte value) =>
        ((int)value).ToUpperCase();

    /// <summary>
    /// Get the uppercase representation of an ASCII char stored as an integer.
    /// </summary>
    public static char ToUpperCase(this char input) =>
        input switch
        {
            >= 'a' and <= 'z' => unchecked((char)(input - 0x20)),
            _ => input
        };

    extension(string? a)
    {
        /// <summary>
        /// Convert a string to a byte array using code page 437.
        /// </summary>
        public byte[] ToBytes()
        {
            if (string.IsNullOrEmpty(a))
                return [];
            
            var result = new byte[a.Length];
            Cp437.CharsToBytes(a, result);
            return result;
        }

        /// <summary>
        /// Convert a string to a byte array using code page 437.
        /// </summary>
        public int ToBytes(Span<byte> destination) => 
            string.IsNullOrEmpty(a) ? 0 : Cp437.CharsToBytes(a, destination);

        /// <summary>
        /// Convert a byte array to a string using code page 437.
        /// </summary>
        public string? UpCased()
        {
            if (a == null)
                return null;

#if NET10_0_OR_GREATER
            return string.Create(a.Length, a, static (span, s) =>
            {
                for (var i = 0; i < s.Length; i++)
                    span[i] = unchecked((char)((int)s[i]).ToUpperCase());
            });
#else
            var str = new char[a.Length];
            for (var i = 0; i < a.Length; i++)
                str[i] = unchecked((char)((int)a[i]).ToUpperCase());
            return new string(str);
#endif
        }
    }

    extension(ReadOnlySpan<char> a)
    {
        /// <summary>
        /// Convert a string to a byte array using code page 437.
        /// </summary>
        public int ToBytes(Span<byte> destination) => 
            a.Length == 0 ? 0 : Cp437.CharsToBytes(a, destination);
    }
    
    extension(ReadOnlySpan<byte> a)
    {
        /// <summary>
        /// Convert a string to a byte array using code page 437.
        /// </summary>
        public string ToStringValue()
        {
            if (a.Length == 0)
                return string.Empty;

#if NET10_0_OR_GREATER
            return string.Create(a.Length, a, static (span, bytes) => Cp437.BytesToChars(bytes, span));
#else
            var destination = new char[a.Length];
            Cp437.BytesToChars(a, destination);
            return new string(destination);
#endif
        }
        
        /// <summary>
        /// Compares source string to another string, with the source UpperCased, and only A-Z.
        /// </summary>
        public bool CaseInsensitiveCharacterEqual(ReadOnlySpan<char> b)
        {
            var i = 0;
            var j = 0;

            if (a.IsEmpty)
                return b.IsEmpty;

            while (i < a.Length)
            {
                var ai = a[i].ToUpperCase();

                if (ai is >= 0x41 and <= 0x5A)
                {
                    if (j >= b.Length)
                        break;

                    if (ai != b[j])
                        return false;
                    j++;
                }

                i++;
            }

            return i == a.Length && j == b.Length;
        }
    }
    
    extension(HWord a)
    {
        /// <summary>
        /// Get the uppercase representation of an ASCII char stored as an integer.
        /// </summary>
        public int ToUpperCase() =>
            (int)a switch
            {
                >= 0x61 and <= 0x7A => a - 0x20,
                _ => a
            };

        /// <summary>
        /// Convert an integer to a character using code page 437.
        /// </summary>
        public char ToChar() => 
            Cp437.ByteToChar(a);
    }
}