using System;
using System.Linq;

namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class VgaGlyphComposer : IGlyphComposer
{
    private readonly int[] _data;
    private readonly int _height;

    public VgaGlyphComposer(IBitmapFont font)
    {
        if (font == null)
            throw new ArgumentNullException(nameof(font));
        
        _data = new int[font.Data.Length];
        for (var i = 0; i < font.Data.Length; i++)
            _data[i] = font.Data.Span[i];

        _height = font.Height;
        MaxWidth = font.Width;
        MaxHeight = font.Height;
    }

    public IGlyph ComposeGlyph(int index)
    {
        var output = new int[_height*8];
        var outputOffset = 0;
        var inputOffset = (index & 0xFF)*_height;
        for (var y = 0; y < _height; y++)
        {
            var bits = _data[inputOffset++];
            for (var x = 0; x < 8; x++)
            {
                output[outputOffset++] = (bits & 0x80) != 0 ? -1 : 0;
                bits <<= 1;
            }
        }
        return new Glyph(index, 8, _height, output);
    }

    public int MaxWidth { get; }
    public int MaxHeight { get; }
}