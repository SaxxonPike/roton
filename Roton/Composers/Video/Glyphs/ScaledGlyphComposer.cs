using System.Linq;

namespace Roton.Composers.Video.Glyphs
{
    public class ScaledGlyphComposer : IGlyphComposer
    {
        private readonly IGlyphComposer _glyphComposer;
        private readonly int _scaleX;
        private readonly int _scaleY;

        public ScaledGlyphComposer(IGlyphComposer glyphComposer, int scaleX, int scaleY)
        {
            _glyphComposer = glyphComposer;
            _scaleX = scaleX;
            _scaleY = scaleY;
            MaxWidth = glyphComposer.MaxWidth*scaleX;
            MaxHeight = glyphComposer.MaxHeight*scaleY;
        }

        public IGlyph ComposeGlyph(int index)
        {
            var glyph = _glyphComposer.ComposeGlyph(index);
            var scaledXScan = glyph.Data.SelectMany(pixel => Enumerable.Repeat(pixel, _scaleX));
            var scaledY = scaledXScan.SelectMany(scan => Enumerable.Repeat(scan, _scaleY));
            return new Glyph(index, glyph.Width * _scaleX, glyph.Height * _scaleY, scaledY);
        }

        public int MaxWidth { get; }
        public int MaxHeight { get; }
    }
}
