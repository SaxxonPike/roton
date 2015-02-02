using System;
using System.Drawing;
using System.IO;

namespace Roton.Common
{
    sealed public class Palette : Roton.Emulation.FixedList<Color>
    {
        /// <summary>
        /// Create a new palette with the default colors.
        /// </summary>
        public Palette()
        {
            Initialize(Properties.Resources.vgapalette);
        }

        /// <summary>
        /// Load a palette from the specified file.
        /// </summary>
        public Palette(string filename)
        {
            Initialize(File.ReadAllBytes(filename));
        }

        /// <summary>
        /// Create a palette from raw data.
        /// </summary>
        public Palette(byte[] source)
        {
            Initialize(source);
        }

        /// <summary>
        /// Load a palette from the specified stream.
        /// </summary>
        public Palette(Stream source)
        {
            byte[] data = new byte[16 * 3];
            source.Read(data, 0, data.Length);
            Initialize(data);
        }

        /// <summary>
        /// Get or set palette colors.
        /// </summary>
        public override Color this[int index]
        {
            get
            {
                return Color.FromArgb(Colors[index & 0xF]);
            }
            set
            {
                Colors[index & 0xF] = value.ToArgb();
            }
        }

        public Int32[] Colors
        {
            get;
            private set;
        }

        /// <summary>
        /// Number of colors in the palette.
        /// </summary>
        public override int Count
        {
            get { return 16; }
        }

        private byte ImportColorValue(int source)
        {
            int result = (source * 255) / 63;
            if (result > 255)
                result = 255;
            if (result < 0)
                result = 0;
            return (byte)(result & 0xFF);
        }

        private void Initialize(byte[] palette)
        {
            if (palette.Length < 48)
            {
                throw Exceptions.InvalidPalette;
            }
            int offset = 0;
            byte red, green, blue;
            this.Colors = new Int32[16];

            for (int i = 0; i < 16; i++)
            {
                red = ImportColorValue(palette[offset++]);
                green = ImportColorValue(palette[offset++]);
                blue = ImportColorValue(palette[offset++]);
                this.Colors[i] = Color.FromArgb(0xFF, red, green, blue).ToArgb();
            }
        }
    }
}
