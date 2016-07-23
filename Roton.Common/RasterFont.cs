﻿using System;
using System.Drawing;
using System.IO;
using Roton.Common.Resources;
using Roton.Core.Collections;

namespace Roton.Common
{
    public sealed class RasterFont : FixedList<IGlyph>, IRasterFont
    {
        /// <summary>
        /// Create a font with the default glyphs.
        /// </summary>
        public RasterFont()
        {
            var resources = new CommonResourceZipFileSystem(Properties.Resources.resources);
            Initialize(resources.GetFont());
        }

        /// <summary>
        /// Load a font from the specified file.
        /// </summary>
        public RasterFont(string filename)
        {
            Initialize(File.ReadAllBytes(filename));
        }

        /// <summary>
        /// Create a font from raw data.
        /// </summary>
        public RasterFont(byte[] font)
        {
            Initialize(font);
        }

        /// <summary>
        /// Load a font from the specified stream. The length must be specified because raw data may lack a header.
        /// </summary>
        public RasterFont(Stream source, int length)
        {
            var font = new byte[length];
            source.Read(font, 0, length);
            Initialize(font);
        }

        public override int Count => 256;

        private byte[] Data { get; set; }

        private Glyph[] Glyphs { get; set; }

        private Glyph[] GlyphsUnscaled { get; set; }

        public int Height { get; private set; }

        private void Initialize(byte[] font)
        {
            byte[] rawFont;

            // attempt to load the font
            try
            {
                if ((font.Length & 0xFF) == 0)
                {
                    // if we have an exact multiple of 256, we likely have a raw font
                    rawFont = font;
                }
                else
                {
                    // check for a few types of compiled font files (usually .COM drivers)
                    if (font[0] == 0xEB && font[1] == 0x47 && font[4] == 0x22)
                    {
                        // font mania 2.2 (quite common in the ZZT community)
                        int fontHeight = font[5];
                        int fontOffset = font[3];
                        fontOffset <<= 8;
                        fontOffset |= font[2];
                        rawFont = new byte[fontHeight*256];
                        Array.Copy(font, fontOffset, rawFont, 0, rawFont.Length);
                    }
                    else if (font[0] == 0xB8 && font[1] == 0x63 && ((font.Length - 139) & 0xFF) == 0)
                    {
                        // fonted 2.0 (lesser known but still needs support)
                        var fontLength = font.Length - 139;
                        var fontOffset = 139;
                        rawFont = new byte[fontLength];
                        Array.Copy(font, fontOffset, rawFont, 0, rawFont.Length);
                    }
                    else
                    {
                        throw Exceptions.InvalidFont;
                    }
                }
            }
            catch (Exception)
            {
                // treat all failures the same- there could be
                // any number of reasons fonts would fail to load
                // but it's not system-critical
                throw Exceptions.InvalidFont;
            }

            // convert the font to a faster format (int+mask)
            Glyphs = new Glyph[256];
            OriginalHeight = rawFont.Length/256;
            OriginalWidth = 8;
            Data = rawFont;
            Rasterize(1, 1);
            GlyphsUnscaled = new Glyph[256];
            for (var i = 0; i < 256; i++)
            {
                GlyphsUnscaled[i] = new Glyph(new byte[OriginalHeight], 1, 1);
                Glyphs[i].Data.CopyTo(GlyphsUnscaled[i].Data, 0);
            }
        }

        public int OriginalHeight { get; private set; }

        public int OriginalWidth { get; private set; }

        private void Rasterize(int xScale, int yScale)
        {
            var glyphData = new byte[OriginalHeight];
            var glyphDataOffset = 0;
            for (var i = 0; i < 256; i++)
            {
                Array.Copy(Data, glyphDataOffset, glyphData, 0, OriginalHeight);
                glyphDataOffset += OriginalHeight;
                Glyphs[i] = new Glyph(glyphData, xScale, yScale);
            }
            Width = OriginalWidth*xScale;
            Height = OriginalHeight*yScale;
        }

        public Bitmap Render(int character, Color foreground, Color background)
        {
            return Glyphs[character & 0xFF].Render(foreground, background);
        }

        public void Render(IFastBitmap bitmap, int x, int y, int character, int foreColor, int backColor)
        {
            Glyphs[character & 0xFF].Render(bitmap, x, y, foreColor, backColor);
        }

        public Bitmap RenderUnscaled(int character, Color foreground, Color background)
        {
            return GlyphsUnscaled[character & 0xFF].Render(foreground, background);
        }

        public void SetScale(int xScale, int yScale)
        {
            Rasterize(xScale, yScale);
        }

        public int Width { get; private set; }
    }
}