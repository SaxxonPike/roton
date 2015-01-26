using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Roton.Windows
{
    sealed public class Glyph : Roton.Emulation.FixedList<int>
    {
        private const Int32 BLACK = -1 ^ COLOR_MASK;
        private const Int32 COLOR_MASK = 0xFFFFFF;
        private const Int32 WHITE = -1;

        internal Glyph(byte[] source, int xScale, int yScale)
        {
            Height = source.Length;
            Width = 8;
            PixelCount = Width * Height * xScale * yScale;
            Data = new Int32[PixelCount];

            int index = 0;
            int rowSize = 8 * xScale;
            for (int i = 0; i < Height; i++)
            {
                byte rowData = source[i];
                for (int j = 0; j < 8; j++)
                {
                    for (int x = 0; x < xScale; x++)
                    {
                        this[index++] = ((rowData & 0x80) != 0) ? WHITE : BLACK;
                    }
                    rowData <<= 1;
                }
                for (int y = 1; y < yScale; y++)
                {
                    for (int x = 0; x < rowSize; x++)
                    {
                        this[index] = this[index - rowSize];
                        index++;
                    }
                }
            }

            Width *= xScale;
            Height *= yScale;
        }

        public override int this[int index]
        {
            get
            {
                return Data[index];
            }
            set
            {
                Data[index] = BLACK | (value & COLOR_MASK);
            }
        }

        public override int Count
        {
            get { return PixelCount; }
        }

        public Int32[] Data
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        private int PixelCount
        {
            get;
            set;
        }

        /// <summary>
        /// Render the glyph using the specified raw color data.
        /// </summary>
        public Int32[] Render(int foreColor, int backColor)
        {
            int length = PixelCount;
            Int32[] result = new Int32[length];
            Int32 mask;

            for (int index = 0; index < length; index++)
            {
                mask = Data[index];
                result[index] = BLACK | (mask & foreColor) | ((mask ^ COLOR_MASK) & backColor);
            }

            return result;
        }

        /// <summary>
        /// Render the glyph to a bitmap using the specified colors.
        /// </summary>
        public Bitmap Render(Color foreColor, Color backColor)
        {
            Bitmap result = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            var bits = result.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(Render(foreColor.ToArgb(), backColor.ToArgb()), 0, bits.Scan0, PixelCount);
            result.UnlockBits(bits);
            return result;
        }

        /// <summary>
        /// Render the glyph to a FastBitmap using the specified color data and coordinates.
        /// </summary>
        public void Render(FastBitmap bitmap, int x, int y, int foreColor, int backColor)
        {
            Int32[] bits = bitmap.Bits;
            Int32[] data = this.Data;
            int bitsWidth = bitmap.Width;
            int bitsOffset = ((y * bitmap.Width) + x);
            int bitsRowOffset;
            int dataOffset = 0;
            int mask;
            int height = this.Height;
            int width = this.Width;
            int i = 0;
            int j = 0;
            while (i < height)
            {
                j = 0;
                bitsRowOffset = bitsOffset;
                while (j < width)
                {
                    mask = data[dataOffset++];
                    bits[bitsOffset++] = BLACK | (mask & foreColor) | ((mask ^ COLOR_MASK) & backColor);
                    j++;
                }
                bitsOffset = bitsRowOffset + bitsWidth;
                i++;
            }
        }

        /// <summary>
        /// Width of the glyph.
        /// </summary>
        public int Width
        {
            get;
            set;
        }
    }
}
