using System.Drawing;
using Roton.Interface.Video;
using Roton.Interface.Video.Glyphs;
using Roton.Interface.Video.Scenes.Composition;

namespace Roton.Interface.Extensions
{
    public static class GlyphExtensions
    {
        public static IDirectAccessBitmap RenderToFastBitmap(this IGlyph glyph, Color foregroundColor, Color backgroundColor)
        {
            var output = new DirectAccessBitmap(glyph.Width, glyph.Height);
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
