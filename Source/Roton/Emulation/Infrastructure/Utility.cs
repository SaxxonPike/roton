using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Roton.Emulation.Infrastructure;

internal static class Utility
{
    private static readonly Encoding CodePage437 = CodePagesEncodingProvider.Instance.GetEncoding(437);

    private static readonly ThreadLocal<byte[]> OneByteArray = new(() => new byte[1]);
    private static readonly ThreadLocal<char[]> OneCharArray = new(() => new char[1]);
    
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
        public char ToChar()
        {
            var bytes = OneByteArray.Value;
            bytes[0] = unchecked((byte)(a & 0xFF));
            var chars = OneCharArray.Value;
            CodePage437.GetChars(bytes, 0, 1, chars, 0);
            return chars[0];
        }

        /// <summary>
        /// Get the uppercase representation of an ASCII char stored as an integer.
        /// </summary>
        [DebuggerStepThrough]
        public int ToUpperCase() =>
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
    public static string ToStringValue(this byte[] value) =>
        CodePage437.GetString(value);

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
        unchecked((char)((int)input).ToUpperCase());

    extension(string a)
    {
        /// <summary>
        /// Compares source string to another string, with the source UpperCased.
        /// </summary>
        [DebuggerStepThrough]
        public bool CaseInsensitiveEqual(string b)
        {
            if (a == null != (b == null))
                return false;

            if (a == null)
                return true;

            if (a.Length != b.Length)
                return false;

            for (var i = 0; i < a.Length; i++)
            {
                if (a[i].ToUpperCase() != b[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Compares source string to another string, with the source UpperCased, and only A-Z.
        /// </summary>
        [DebuggerStepThrough]
        public bool CaseInsensitiveCharacterEqual(string b)
        {
            var i = 0;
            var j = 0;

            if (a == null != (b == null))
                return false;

            if (a == null)
                return true;

            while (i < a.Length)
            {
                var ai = a[i].ToUpperCase();

                if (ai >= 0x41 && ai <= 0x5A)
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

        /// <summary>
        /// Convert a string to a byte array using code page 437.
        /// </summary>
        [DebuggerStepThrough]
        public byte[] ToBytes() =>
            string.IsNullOrEmpty(a)
                ? []
                : CodePage437.GetBytes(a);
    }
}