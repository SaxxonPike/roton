using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public static class ParserExtensions
{
    extension(IParser parser)
    {
        /// <summary>
        /// See <see cref="IParser.ReadLine"/>.
        /// </summary>
        public string ReadLine(int index, ref Word instruction)
        {
            var buffer = (stackalloc char[byte.MaxValue]);
            return parser.ReadLine(index, ref instruction, buffer).ToString();
        }

        /// <summary>
        /// See <see cref="IParser.ReadWord"/>.
        /// </summary>
        public void ReadWord(int index, ref Word instruction)
        {
            var result = (stackalloc char[byte.MaxValue]);
            parser.ReadWord(index, ref instruction, result);
        }

        /// <summary>
        /// See <see cref="IParser.ReadLine"/>. The result is discarded.
        /// </summary>
        public void DiscardLine(int index, ref Word instruction)
        {
            var buffer = (stackalloc char[byte.MaxValue]);
            parser.ReadLine(index, ref instruction, buffer);
        }
    }
}