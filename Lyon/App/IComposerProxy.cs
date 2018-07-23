using Roton.Composers.Audio;
using Roton.Composers.Video;
using Roton.Composers.Video.Glyphs;
using Roton.Composers.Video.Palettes;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Data;

namespace Lyon.App
{
    public interface IComposerProxy
    {
        event SetSizeEventHandler AfterSetSize;
        
        void SetFont(byte[] data);
        void SetPalette(byte[] data);
        void SetScene(int width, int height, bool wide);
        void SetSound(IDrumBank drumBank, int sampleRate, int samplesPerDrumTick);
        IGlyphComposer GlyphComposer { get; }
        IPaletteComposer PaletteComposer { get; }
        IBitmapSceneComposer SceneComposer { get; }
        IAudioComposer AudioComposer { get; }
    }
}