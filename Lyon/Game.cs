using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Input;
using Roton.Core;
using Roton.FileIo;
using Roton.Interface.Audio.Composition;
using Roton.Interface.Input;
using Roton.Interface.Resources;
using Roton.Interface.Video;
using Roton.Interface.Video.Glyphs;
using Roton.Interface.Video.Palettes;
using Roton.Interface.Video.Scenes.Composition;
using Roton.Interface.Video.Scenes.Presentation;

namespace Lyon
{
    public class Game
    {
        private class Window : GameWindow, IOpenGlScenePresenterWindow
        {
            private readonly Func<IContext> _getContext;
            private readonly OpenTkKeyBuffer _openTkKeyBuffer;
            private readonly IOpenGlScenePresenter _presenter;

            public Window(
                Func<IBitmapSceneComposer> getComposer,
                Func<IContext> getContext,
                OpenTkKeyBuffer openTkKeyBuffer)
            {
                _getContext = getContext;
                _openTkKeyBuffer = openTkKeyBuffer;
                _presenter = new OpenGlScenePresenter(getComposer, () => this);
            }

            public void SetSize(int width, int height)
            {
                Width = width;
                Height = height;
                _presenter.UpdateViewport();
            }

            private void UpdateKey(KeyboardKeyEventArgs e)
            {
                _openTkKeyBuffer.Alt = e.Alt;
                _openTkKeyBuffer.Control = e.Control;
                _openTkKeyBuffer.Shift = e.Shift;
            }

            protected override void OnKeyDown(KeyboardKeyEventArgs e)
            {
                _openTkKeyBuffer.Press(e.Key);
                UpdateKey(e);
                base.OnKeyDown(e);
            }

            protected override void OnKeyPress(KeyPressEventArgs e)
            {
                _openTkKeyBuffer.Press(e.KeyChar);
                base.OnKeyPress(e);
            }

            protected override void OnUpdateFrame(FrameEventArgs e)
            {
                _getContext().ExecuteOnce();
                base.OnUpdateFrame(e);
            }

            protected override void OnRenderFrame(FrameEventArgs e)
            {
                _presenter.Render();
                base.OnRenderFrame(e);
            }
        }

        private Window _window;
        private IBitmapSceneComposer _sceneComposer;
        private IContext _context;
        private readonly IGlyphComposer _glyphComposer;
        private readonly IPaletteComposer _paletteComposer;
        private readonly OpenTkKeyBuffer _openTkKeyBuffer;
        private IAudioComposer _audioComposer;
        private Action _initializeContext;

        public Game()
        {
            var fontData = CommonResourceZipFileSystem.Default.GetFont();
            var paletteData = CommonResourceZipFileSystem.Default.GetPalette();
            _glyphComposer = new AutoDetectBinaryGlyphComposer(fontData);
            _paletteComposer = new VgaPaletteComposer(paletteData);
            _openTkKeyBuffer = new OpenTkKeyBuffer();
        }

        private void InitializeSceneComposer(int width, int height, bool wide)
        {
            var oldSceneComposer = _sceneComposer;
            _sceneComposer = new BitmapSceneComposer(_glyphComposer, _paletteComposer, width, height);
            var newWidth = width * _glyphComposer.MaxWidth * (wide ? 2 : 1);
            var newHeight = height * _glyphComposer.MaxHeight;
            _window?.SetSize(newWidth, newHeight);
            oldSceneComposer?.Dispose();
        }

        private EngineConfiguration GetDefaultConfiguration(IFileSystem fileSystem)
        {
            return new EngineConfiguration
            {
                Disk = fileSystem,
                EditorMode = false,
                Keyboard = _openTkKeyBuffer,
                RandomSeed = 0,
                Speaker = GetAudioComposer(),
                Terminal = GetSceneComposer()
            };
        }

        private void InitializeEmptyContext()
        {
            _context?.Stop();
            _context = new Context(GetDefaultConfiguration(new DiskFileSystem()), ContextEngine.Zzt);
            _context.Start();
        }

        private void InitializeContextFromStream(Stream stream)
        {
            _context?.Stop();
            _context = new Context(GetDefaultConfiguration(new DiskFileSystem()), stream);
            _context.Start();
        }

        private IContext GetContext()
        {
            if (_context != null)
                return _context;

            _initializeContext();
            return _context;
        }

        private IBitmapSceneComposer GetSceneComposer()
        {
            if (_sceneComposer != null)
                return _sceneComposer;

            InitializeSceneComposer(80, 25, false);
            return _sceneComposer;
        }

        private IAudioComposer GetAudioComposer()
        {
            if (_audioComposer != null)
                return _audioComposer;

            InitializeAudioComposer();
            return _audioComposer;
        }

        private void InitializeAudioComposer()
        {
            _audioComposer = new AudioComposer(() => GetContext().Drums, 44100, 500);
        }

        private void RunCommon()
        {
            _window = _window ?? new Window(GetSceneComposer, GetContext, _openTkKeyBuffer);
            _window.Run();
            _context?.Stop();
        }

        public void Run()
        {
            _initializeContext = InitializeEmptyContext;
            RunCommon();
        }

        public void Run(Stream stream)
        {
            _initializeContext = () => InitializeContextFromStream(stream);
            RunCommon();
        }

        public void Run(string path)
        {
            _initializeContext = () => InitializeContextFromPath(path);
            RunCommon();
        }

        private void InitializeContextFromPath(string path)
        {
            _context?.Stop();
            _context = new Context(GetDefaultConfiguration(new DiskFileSystem(Path.GetDirectoryName(path))), File.ReadAllBytes(path));
            _context.Start();
        }
    }
}
