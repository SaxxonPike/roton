using System;
using System.IO;
using OpenTK;
using OpenTK.Input;
using Roton.Core;
using Roton.FileIo;
using Roton.Interface.Audio.Composition;
using Roton.Interface.Events;
using Roton.Interface.Input;
using Roton.Interface.Resources;
using Roton.Interface.Video;
using Roton.Interface.Video.Glyphs;
using Roton.Interface.Video.Palettes;
using Roton.Interface.Video.Scenes.Composition;
using Roton.Interface.Video.Scenes.Presentation;

namespace Lyon.App
{
    public class Game : IGame
    {
        private class Window : GameWindow, IOpenGlScenePresenterWindow
        {
            private readonly Func<IContext> _getContext;
            private readonly OpenTkKeyBuffer _openTkKeyBuffer;
            private readonly IOpenGlScenePresenter _presenter;

            public Window(
                IComposerProxy composerProxy,
                Func<IContext> getContext,
                OpenTkKeyBuffer openTkKeyBuffer)
            {
                _getContext = getContext;
                _openTkKeyBuffer = openTkKeyBuffer;
                _presenter = new OpenGlScenePresenter(() => composerProxy.SceneComposer, () => this);
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

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                _presenter.UpdateViewport();
            }
        }

        private Window _window;
        private IContext _context;
        private readonly IComposerProxy _composerProxy;
        private readonly IOpenTkKeyBuffer _keyboard;
        private Action _initializeContext;

        public Game(
            IComposerProxy composerProxy,
            IOpenTkKeyBuffer keyboard)
        {
            _composerProxy = composerProxy;
            _keyboard = keyboard;

            _composerProxy.AfterSetSize += OnSetSize;
        }

        private void SetSize(int width, int height, bool wide)
        {
            var newWidth = width * _composerProxy.GlyphComposer.MaxWidth * (wide ? 2 : 1);
            var newHeight = height * _composerProxy.GlyphComposer.MaxHeight;
            _window?.SetSize(newWidth, newHeight);
        }

        private void OnSetSize(object sender, SetSizeEventArgs e)
        {
            SetSize(e.Width, e.Height, e.Wide);
        }

        private EngineConfiguration GetDefaultConfiguration()
        {
            return new EngineConfiguration
            {
                EditorMode = false,
                RandomSeed = 0
            };
        }

        private void InitializeEmptyContext()
        {
            _context?.Stop();
            _context = new Context(GetDefaultConfiguration(), ContextEngine.Zzt);
            _context.Start();
        }

        private void InitializeContextFromStream(Stream stream)
        {
            _context?.Stop();
            _context = new Context(GetDefaultConfiguration(), stream);
            _context.Start();
        }

        private IContext GetContext()
        {
            if (_context != null)
                return _context;

            _initializeContext();
            return _context;
        }

        private void RunCommon()
        {
            _window = _window ?? new Window(_composerProxy, GetContext, _keyboard);
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
