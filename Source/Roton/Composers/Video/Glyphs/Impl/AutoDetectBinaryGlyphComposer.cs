using System;

namespace Roton.Composers.Video.Glyphs.Impl;

public sealed class AutoDetectBinaryGlyphComposer : IGlyphComposer
{
    private readonly IGlyphComposer? _innerGlyphComposer;

    public AutoDetectBinaryGlyphComposer(ReadOnlyMemory<byte> sourceData)
    {
        var bytes = sourceData.Span;
        IBitmapFont? font = null;

        if ((sourceData.Length & 0xFF) == 0)
        {
            // if we have an exact multiple of 256, we likely have a raw font
            font = new BitmapFont(sourceData, 8, bytes.Length >> 8);
        }
        else
        {
            // check for a few types of compiled font files (usually .COM drivers)
            if (bytes[0] == 0xEB && bytes[1] == 0x47 && bytes[4] == 0x22)
            {
                // font mania 2.2 (quite common in the ZZT community)
                int fontHeight = bytes[5];
                int fontOffset = bytes[3];
                fontOffset <<= 8;
                fontOffset |= bytes[2];
                font = new BitmapFont(sourceData.Slice(fontOffset, fontHeight * 256), 8, fontHeight);
            }
            else if (bytes[0] == 0xB8 && bytes[1] == 0x63 && ((bytes.Length - 139) & 0xFF) == 0)
            {
                // fonted 2.0 (lesser known but still needs support)
                var fontLength = bytes.Length - 139;
                font = new BitmapFont(sourceData.Slice(139, fontLength), 8, fontLength >> 8);
            }
        }

        _innerGlyphComposer = font == null ? null : new VgaGlyphComposer(font);

        if (font == null)
            return;

        MaxWidth = font.Width;
        MaxHeight = font.Height;
    }

    public Glyph? ComposeGlyph(int index) =>
        _innerGlyphComposer?.ComposeGlyph(index);

    public int MaxWidth { get; }
    public int MaxHeight { get; }
}