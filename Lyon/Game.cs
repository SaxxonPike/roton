using System;
using System.Windows.Forms;
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
using KeyPressEventArgs = OpenTK.KeyPressEventArgs;

namespace Lyon
{
    public class Game
    {
        private class Window : GameWindow, IOpenGlScenePresenterWindow
        {
            private readonly Func<IContext> _getContext;
            private readonly KeysBuffer _keysBuffer;
            private readonly IOpenGlScenePresenter _presenter;

            public Window(
                Func<IBitmapSceneComposer> getComposer,
                Func<IContext> getContext,
                KeysBuffer keysBuffer)
            {
                _getContext = getContext;
                _keysBuffer = keysBuffer;
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
                _keysBuffer.Alt = e.Alt;
                _keysBuffer.Control = e.Control;
                _keysBuffer.Shift = e.Shift;
            }

            protected override void OnKeyDown(KeyboardKeyEventArgs e)
            {
                _keysBuffer.Press(e.Key);
                UpdateKey(e);
                base.OnKeyDown(e);
            }

            protected override void OnKeyPress(KeyPressEventArgs e)
            {
                _keysBuffer.Press(e.KeyChar);
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
        private readonly KeysBuffer _keysBuffer;
        private IAudioComposer _audioComposer;

        public Game()
        {
            var fontData = CommonResourceZipFileSystem.Default.GetFont();
            var paletteData = CommonResourceZipFileSystem.Default.GetPalette();
            _glyphComposer = new AutoDetectBinaryGlyphComposer(fontData);
            _paletteComposer = new VgaPaletteComposer(paletteData);
            _keysBuffer = new KeysBuffer();
        }

        private void InitializeSceneComposer(int width, int height, bool wide)
        {
            _sceneComposer = new BitmapSceneComposer(_glyphComposer, _paletteComposer, width, height);
            var newWidth = width*_glyphComposer.MaxWidth*(wide ? 2 : 1);
            var newHeight = height*_glyphComposer.MaxHeight;
            _window?.SetSize(newWidth, newHeight);
        }

        private void InitializeContext()
        {
            _context?.Stop();

            _context = new Context(new EngineConfiguration
            {
                Disk = new DiskFileSystem(),
                EditorMode = false,
                Keyboard = _keysBuffer,
                RandomSeed = 0,
                Speaker = GetAudioComposer(),
                Terminal = GetSceneComposer()
            }, ContextEngine.Zzt);

            _context.Start();
        }

        private IContext GetContext()
        {
            if (_context != null)
                return _context;

            InitializeContext();
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

        public void Run()
        {
            _window = _window ?? new Window(GetSceneComposer, GetContext, _keysBuffer);
            _window.Run();
            _context?.Stop();
        }
    }
}
