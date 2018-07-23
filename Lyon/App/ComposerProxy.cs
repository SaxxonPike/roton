﻿using System;
using Roton.Composers.Audio;
using Roton.Composers.Video;
using Roton.Composers.Video.Glyphs;
using Roton.Composers.Video.Palettes;
using Roton.Composers.Video.Scenes;
using Roton.Emulation.Data;

namespace Lyon.App
{
    public class ComposerProxy : IComposerProxy
    {
        private Lazy<IGlyphComposer> _glyphComposer;
        private Lazy<IPaletteComposer> _paletteComposer;
        private Lazy<IBitmapSceneComposer> _sceneComposer;
        private Lazy<IAudioComposer> _audioComposer;

        public event SetSizeEventHandler AfterSetSize;

        public void SetSound(IDrumBank drumBank, int sampleRate, int samplesPerDrumTick)
        {
            _audioComposer = new Lazy<IAudioComposer>(() =>
                new AudioComposer(drumBank, sampleRate, samplesPerDrumTick));
        }
        
        public void SetFont(byte[] data)
        {
            _glyphComposer = new Lazy<IGlyphComposer>(() => 
                new CachedGlyphComposer(new AutoDetectBinaryGlyphComposer(data)));
        }

        public void SetPalette(byte[] data)
        {
            _paletteComposer = new Lazy<IPaletteComposer>(() => 
                new CachedPaletteComposer(new VgaPaletteComposer(data)));
        }

        public void SetScene(int width, int height, bool wide)
        {
            if (_sceneComposer?.IsValueCreated ?? false)
                _sceneComposer.Value?.Dispose();

            _sceneComposer = new Lazy<IBitmapSceneComposer>(() =>
            {
                var result = new BitmapSceneComposer(GlyphComposer, PaletteComposer);
                result.AfterSetSize += (sender, e) => AfterSetSize?.Invoke(sender, e);
                return result;
            });
        }
        
        public IGlyphComposer GlyphComposer => _glyphComposer?.Value;
        public IPaletteComposer PaletteComposer => _paletteComposer?.Value;
        public IBitmapSceneComposer SceneComposer => _sceneComposer?.Value;
        public IAudioComposer AudioComposer => _audioComposer?.Value;
    }
}