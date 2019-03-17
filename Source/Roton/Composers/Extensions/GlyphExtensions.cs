using System.Drawing;
using Roton.Composers.Video.Glyphs;
using Roton.Composers.Video.Scenes;
using Roton.Composers.Video.Scenes.Impl;

namespace Roton.Composers.Extensions
{
    public static class GlyphExtensions
    {
        public static IBitmap RenderToFastBitmap(this IGlyph glyph, Color foregroundColor, Color backgroundColor)
        {
            var output = new Bitmap(glyph.Width, glyph.Height);
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
