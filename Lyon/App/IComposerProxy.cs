using Roton.Core;
using Roton.Emulation.Data;
using Roton.Interface.Audio.Composition;
using Roton.Interface.Events;
using Roton.Interface.Video.Glyphs;
using Roton.Interface.Video.Palettes;
using Roton.Interface.Video.Scenes.Composition;

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