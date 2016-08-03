using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Roton.Core.Collections;
using Roton.Interface.Resources;

namespace Roton.Interface.Video
{
    public sealed class Palette : FixedList<Color>, IPalette
    {
        /// <summary>
        ///     Create a new palette with the default colors.
        /// </summary>
        public Palette()
        {
            var resources = new CommonResourceZipFileSystem(Properties.Resources.resources);
            Initialize(resources.GetPalette());
        }

        /// <summary>
        ///     Load a palette from the specified file.
        /// </summary>
        public Palette(string filename)
        {
            Initialize(File.ReadAllBytes(filename));
        }

        /// <summary>
        ///     Create a palette from raw data.
        /// </summary>
        public Palette(IList<byte> source)
        {
            Initialize(source);
        }

        /// <summary>
        ///     Load a palette from the specified stream.
        /// </summary>
        public Palette(Stream source)
        {
            var data = new byte[16*3];
            source.Read(data, 0, data.Length);
            Initialize(data);
        }

        public int[] Colors { get; private set; }

        /// <summary>
        ///     Number of colors in the palette.
        /// </summary>
        public override int Count => 16;

        protected override Color GetItem(int index)
        {
            return Color.FromArgb(Colors[index & 0xF]);
        }

        private byte ImportColorValue(int source)
        {
            var result = source*255/63;
            if (result > 255)
                result = 255;
            if (result < 0)
                result = 0;
            return (byte) (result & 0xFF);
        }

        private void Initialize(IList<byte> palette)
        {
            if (palette.Count < 48)
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

        protected override void SetItem(int index, Color value)
        {
            Colors[index & 0xF] = value.ToArgb();
        }
    }
}