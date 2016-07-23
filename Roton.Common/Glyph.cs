using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Roton.Core.Collections;

namespace Roton.Common
{
    public sealed class Glyph : FixedList<int>, IGlyph
    {
        private const int Black = -1 ^ ColorMask;
        private const int ColorMask = 0xFFFFFF;
        private const int White = -1;

        internal Glyph(IList<byte> source, int xScale, int yScale)
        {
            Height = source.Count;
            Width = 8;
            PixelCount = Width*Height*xScale*yScale;
            Data = new int[PixelCount];

            var index = 0;
            var rowSize = 8*xScale;
            for (var i = 0; i < Height; i++)
            {
                var rowData = source[i];
                for (var j = 0; j < 8; j++)
                {
                    for (var x = 0; x < xScale; x++)
                    {
                        this[index++] = (rowData & 0x80) != 0 ? White : Black;
                    }
                    rowData <<= 1;
                }
                for (var y = 1; y < yScale; y++)
                {
                    for (var x = 0; x < rowSize; x++)
                    {
                        this[index] = this[index - rowSize];
                        index++;
                    }
                }
            }

            Width *= xScale;
            Height *= yScale;
        }

        public override int Count => PixelCount;

        private int PixelCount { get; }

        public int[] Data { get; }

        public int Height { get; }

        /// <summary>
        ///     Render the glyph using the specified raw color data.
        /// </summary>
        public int[] Render(int foreColor, int backColor)
        {
            var length = PixelCount;
            var result = new int[length];

            for (var index = 0; index < length; index++)
            {
                var mask = Data[index];
                result[index] = Black | (mask & foreColor) | ((mask ^ ColorMask) & backColor);
            }

            return result;
        }

        /// <summary>
        ///     Render the glyph to a bitmap using the specified colors.
        /// </summary>
        public Bitmap Render(Color foreColor, Color backColor)
        {
            var result = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            var bits = result.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly,
                PixelFormat.Format32bppPArgb);
            Marshal.Copy(Render(foreColor.ToArgb(), backColor.ToArgb()), 0, bits.Scan0, PixelCount);
            result.UnlockBits(bits);
            return result;
        }

        /// <summary>
        ///     Render the glyph to a FastBitmap using the specified color data and coordinates.
        /// </summary>
        public void Render(IFastBitmap bitmap, int x, int y, int foreColor, int backColor)
        {
            var bits = bitmap.Bits;
            var data = Data;
            var bitsWidth = bitmap.Width;
            var bitsOffset = y*bitmap.Width + x;
            var dataOffset = 0;
            var height = Height;
            var width = Width;
            var i = 0;
            while (i < height)
            {
                var j = 0;
                var bitsRowOffset = bitsOffset;
                while (j < width)
                {
                    var mask = data[dataOffset++];
                    bits[bitsOffset++] = Black | (mask & foreColor) | ((mask ^ ColorMask) & backColor);
                    j++;
                }
                bitsOffset = bitsRowOffset + bitsWidth;
                i++;
            }
        }

        /// <summary>
        ///     Width of the glyph.
        /// </summary>
        public int Width { get; }

        protected override int GetItem(int index)
        {
            return Data[index];
        }

        protected override void SetItem(int index, int value)
        {
            Data[index] = Black | (value & ColorMask);
        }
    }
}