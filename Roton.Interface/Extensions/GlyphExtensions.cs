using System.Drawing;
using Roton.Interface.Video;
using Roton.Interface.Video.Glyphs;

namespace Roton.Interface.Extensions
{
    public static class GlyphExtensions
    {
        public static IFastBitmap RenderToFastBitmap(this IGlyph glyph, Color foregroundColor, Color backgroundColor)
        {
            var output = new FastBitmap(glyph.Width, glyph.Height);
            var count = output.Bits.Length;
            var foregroundBits = foregroundColor.ToArgb();
            var backgroundBits = backgroundColor.ToArgb();
            var inputBits = glyph.Data;
            var outputBits = output.Bits;

            for (var index = 0; index < count; index++)
            {
                var inputBitData = inputBits[index];
                outputBits[index] = (inputBitData & foregroundBits) | (~inputBitData & backgroundBits);
            }

            return output;
        }
    }
}
