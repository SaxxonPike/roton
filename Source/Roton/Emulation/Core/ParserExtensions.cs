using System;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

internal static class ParserExtensions
{
    extension(IParser parser)
    {
        /// <summary>
        /// See <see cref="IParser.ReadWord(ReadOnlySpan{char}, ref Word, Span{char})"/>.
        /// </summary>
        public void ReadWord(ReadOnlySpan<char> code, ref Word instruction)
        {
            var result = (stackalloc char[byte.MaxValue]);
            parser.ReadWord(code, ref instruction, result);
        }

        /// <summary>
        /// See <see cref="IParser.ReadLine"/>. The result is discarded.
        /// </summary>
        /// <remarks>
        /// RoZ: OopSkipLine
        /// </remarks>
        public void DiscardLine(ReadOnlySpan<char> code, ref Word instruction)
        {
            var buffer = (stackalloc char[byte.MaxValue]);
            parser.ReadLine(code, ref instruction, buffer);
        }
    }
}