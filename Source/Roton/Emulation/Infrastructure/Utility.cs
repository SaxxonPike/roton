using System.Diagnostics;
using System.Text;

namespace Roton.Emulation.Infrastructure
{
    internal static class Utility
    {
        private static readonly Encoding CodePage437 = CodePagesEncodingProvider.Instance.GetEncoding(437);

        /// <summary>
        ///     Return the absolute difference between this value and another specified value.
        /// </summary>
        [DebuggerStepThrough]
        public static int AbsDiff(this int a, int b)
        {
            var diff = a - b;
            if (diff < 0)
                return -diff;
            return diff;
        }

        /// <summary>
        ///     Return 1 if the value is above zero, -1 if the value is below zero, and 0 otherwise.
        /// </summary>
        [DebuggerStepThrough]
        public static int Polarity(this int value)
        {
            if (value > 0)
                return 1;
            if (value < 0)
                return -1;
            return 0;
        }

        /// <summary>
        ///     Return the squared result of an integer.
        /// </summary>
        [DebuggerStepThrough]
        public static int Square(this int i)
        {
            return i*i;
        }

        /// <summary>
        ///     Convert a string to a byte array using code page 437.
        /// </summary>
        [DebuggerStepThrough]
        public static byte[] ToBytes(this string value)
        {
            return string.IsNullOrEmpty(value) 
                ? new byte[0] 
                : CodePage437.GetBytes(value);
        }

        /// <summary>
        ///     Convert an integer to a character using code page 437.
        /// </summary>
        [DebuggerStepThrough]
        public static char ToChar(this int value)
        {
            return CodePage437.GetChars(new[] {(byte) (value & 0xFF)})[0];
        }

        /// <summary>
        ///     Get the lowercase representation of an ASCII char stored as a byte.
        /// </summary>
        [DebuggerStepThrough]
        public static int ToLowerCase(this byte value)
        {
            if (value >= 0x41 && value <= 0x5A)
            {
                value -= 0x20;
            }
            return value;
        }

        /// <summary>
        ///     Convert an integer to a string using code page 437.
        /// </summary>
        [DebuggerStepThrough]
        public static string ToStringValue(this int value)
        {
            return CodePage437.GetString(new[] {(byte) (value & 0xFF)});
        }

        /// <summary>
        ///     Convert a byte array to a string using code page 437.
        /// </summary>
        [DebuggerStepThrough]
        public static string ToStringValue(this byte[] value)
        {
            return CodePage437.GetString(value);
        }

        /// <summary>
        ///     Get the uppercase representation of an input key.
        /// </summary>
        [DebuggerStepThrough]
        public static EngineKeyCode ToUpperCase(this EngineKeyCode value)
        {
            return (EngineKeyCode) ((int) value).ToUpperCase();
        }

        /// <summary>
        ///     Get the uppercase representation of an ASCII char stored as a byte.
        /// </summary>
        [DebuggerStepThrough]
        public static int ToUpperCase(this byte value) => ToUpperCase((int) value);

        /// <summary>
        ///     Get the uppercase representation of an ASCII char stored as an integer.
        /// </summary>
        [DebuggerStepThrough]
        public static int ToUpperCase(this int value)
        {
            if (value >= 0x61 && value <= 0x7A)
            {
                value -= 0x20;
            }
            return value;
        }
        
        /// <summary>
        ///     Get the uppercase representation of an ASCII char stored as an integer.
        /// </summary>
        [DebuggerStepThrough]
        public static char ToUpperCase(this char input)
        {
            unchecked
            {
                return (char) ToUpperCase((int)input);
            }
        }

        /// <summary>
        /// Compares source string to another string, with the source UpperCased.
        /// </summary>
        [DebuggerStepThrough]
        public static bool CaseInsensitiveEqual(this string a, string b)
        {
            if ((a == null) != (b == null))
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
        public static bool CaseInsensitiveCharacterEqual(this string a, string b)
        {
            var i = 0;
            var j = 0;
            
            if ((a == null) != (b == null))
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
    }
}