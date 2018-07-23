using System.Linq;

namespace Roton.Composers.Video.Glyphs
{
    public class AutoDetectBinaryGlyphComposer : IGlyphComposer
    {
        private readonly IGlyphComposer _innerGlyphComposer;

        public AutoDetectBinaryGlyphComposer(byte[] sourceData)
        {
            BitmapFont font = null;

            if ((sourceData.Length & 0xFF) == 0)
            {
                // if we have an exact multiple of 256, we likely have a raw font
                font = new BitmapFont(sourceData, 8, sourceData.Length >> 8);
            }
            else
            {
                // check for a few types of compiled font files (usually .COM drivers)
                if (sourceData[0] == 0xEB && sourceData[1] == 0x47 && sourceData[4] == 0x22)
                {
                    // font mania 2.2 (quite common in the ZZT community)
                    int fontHeight = sourceData[5];
                    int fontOffset = sourceData[3];
                    fontOffset <<= 8;
                    fontOffset |= sourceData[2];
                    font = new BitmapFont(sourceData.Skip(fontOffset).Take(fontHeight * 256).ToArray(), 8, fontHeight);
                }
                else if (sourceData[0] == 0xB8 && sourceData[1] == 0x63 && ((sourceData.Length - 139) & 0xFF) == 0)
                {
                    // fonted 2.0 (lesser known but still needs support)
                    var fontLength = sourceData.Length - 139;
                    font = new BitmapFont(sourceData.Skip(139).Take(fontLength).ToArray(), 8, fontLength >> 8);
                }
            }

            _innerGlyphComposer = new VgaGlyphComposer(font);
            
            if (font == null) 
                return;
            
            MaxWidth = font.Width;
            MaxHeight = font.Height;
        }

        public IGlyph ComposeGlyph(int index) => _innerGlyphComposer.ComposeGlyph(index);
        public int MaxWidth { get; }
        public int MaxHeight { get; }
    }
}
