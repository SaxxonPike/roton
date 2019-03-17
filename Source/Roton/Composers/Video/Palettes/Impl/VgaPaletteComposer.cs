using System.Drawing;

namespace Roton.Composers.Video.Palettes.Impl
{
    public sealed class VgaPaletteComposer : IPaletteComposer
    {
        private readonly byte[] _data;

        public VgaPaletteComposer(byte[] data)
        {
            _data = data;
        }

        public Color ComposeColor(int index)
        {
            var offset = (index & 0xF)*3;
            var red = (int) _data[offset];
            var green = (int) _data[offset + 1];
            var blue = (int) _data[offset + 2];
            var adjustedRed = (red << 2) | (red >> 4);
            var adjustedGreen = (green << 2) | (green >> 4);
            var adjustedBlue = (blue << 2) | (blue >> 4);
            return Color.FromArgb(adjustedRed, adjustedGreen, adjustedBlue);
        }
    }
}