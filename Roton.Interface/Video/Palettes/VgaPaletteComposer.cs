using OpenTK.Graphics;

namespace Roton.Interface.Video.Palettes
{
    public class VgaPaletteComposer : IPaletteComposer
    {
        private readonly byte[] _data;

        public VgaPaletteComposer(byte[] data)
        {
            _data = data;
        }

        public Color4 ComposeColor(int index)
        {
            var offset = (index & 0xF)*3;
            var red = (int) _data[offset];
            var green = (int) _data[offset + 1];
            var blue = (int) _data[offset + 2];
            var adjustedRed = (red << 2) | (red >> 4);
            var adjustedGreen = (green << 2) | (green >> 4);
            var adjustedBlue = (blue << 2) | (blue >> 4);
            return new Color4(adjustedRed / 256f, adjustedGreen / 256f, adjustedBlue / 256f, 1.0f);
        }
    }
}