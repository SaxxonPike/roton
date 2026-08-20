using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Infrastructure;

internal static class Utility
{
    extension(int a)
    {
        /// <summary>
        /// Return the absolute difference between this value and another specified value.
        /// </summary>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int AbsDiff(int b) =>
            Math.Abs(a - b);

        /// <summary>
        /// Return 1 if the value is above zero, -1 if the value is below zero, and 0 otherwise.
        /// </summary>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Polarity() =>
            Math.Sign(a);

        /// <summary>
        /// Return the squared result of an integer.
        /// </summary>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Square() =>
            a * a;

        /// <summary>
        /// Convert an integer to a character using code page 437.
        /// </summary>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char ToChar() => 
            Cp437.ByteToChar(unchecked((byte)a));

        /// <summary>
        /// Get the uppercase representation of an ASCII char stored as an integer.
        /// </summary>
        [DebuggerStepThrough]
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
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EngineKeyCode ToUpperCase(this EngineKeyCode value) =>
        (EngineKeyCode)((int)value).ToUpperCase();

    /// <summary>
    /// Get the uppercase representation of an ASCII char stored as a byte.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ToUpperCase(this byte value) =>
        ((int)value).ToUpperCase();

    /// <summary>
    /// Get the uppercase representation of an ASCII char stored as an integer.
    /// </summary>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [DebuggerStepThrough]
        public byte[] ToBytes()
        {
            if (string.IsNullOrEmpty(a))
                return [];
            
            var result = new byte[a!.Length];
            Cp437.CharsToBytes(a, result);
            return result;
        }

        /// <summary>
        /// Convert a string to a byte array using code page 437.
        /// </summary>
        [DebuggerStepThrough]
        public int ToBytes(Span<byte> destination) => 
            string.IsNullOrEmpty(a) ? 0 : Cp437.CharsToBytes(a, destination);

        /// <summary>
        /// Convert a byte array to a string using code page 437.
        /// </summary>
        [DebuggerStepThrough]
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
        [DebuggerStepThrough]
        public int ToBytes(Span<byte> destination) => 
            a.Length == 0 ? 0 : Cp437.CharsToBytes(a, destination);
    }
    
    extension(ReadOnlySpan<byte> a)
    {
        /// <summary>
        /// Convert a string to a byte array using code page 437.
        /// </summary>
        [DebuggerStepThrough]
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
        [DebuggerStepThrough]
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
        [DebuggerStepThrough]
        public int ToUpperCase() =>
            (int)a switch
            {
                >= 0x61 and <= 0x7A => a - 0x20,
                _ => a
            };

        /// <summary>
        /// Convert an integer to a character using code page 437.
        /// </summary>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public char ToChar() => 
            Cp437.ByteToChar(a);
    }
}