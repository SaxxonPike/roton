using System.Drawing;
using System.IO;
using Roton.Common.Properties;
using Roton.Core;

namespace Roton.Common
{
    public sealed class Palette : FixedList<Color>
    {
        /// <summary>
        /// Create a new palette with the default colors.
        /// </summary>
        public Palette()
        {
            Initialize(Resources.vgapalette);
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
            var data = new byte[16*3];
            source.Read(data, 0, data.Length);
            Initialize(data);
        }

        protected override Color GetItem(int index)
        {
            return Color.FromArgb(Colors[index & 0xF]);
        }

        protected override void SetItem(int index, Color value)
        {
            Colors[index & 0xF] = value.ToArgb();
        }

        public int[] Colors { get; private set; }

        /// <summary>
        /// Number of colors in the palette.
        /// </summary>
        public override int Count => 16;

        private byte ImportColorValue(int source)
        {
            var result = source*255/63;
            if (result > 255)
                result = 255;
            if (result < 0)
                result = 0;
            return (byte) (result & 0xFF);
        }

        private void Initialize(byte[] palette)
        {
            if (palette.Length < 48)
            {
                throw Exceptions.InvalidPalette;
            }
            var offset = 0;
            Colors = new int[16];

            for (var i = 0; i < 16; i++)
            {
                var red = ImportColorValue(palette[offset++]);
                var green = ImportColorValue(palette[offset++]);
                var blue = ImportColorValue(palette[offset++]);
                Colors[i] = Color.FromArgb(0xFF, red, green, blue).ToArgb();
            }
        }
    }
}