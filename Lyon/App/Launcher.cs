using System;
using OpenTK;
using OpenTK.Input;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Interface.Events;
using Roton.Interface.Input;
using Roton.Interface.Video;
using Roton.Interface.Video.Scenes.Presentation;

namespace Lyon.App
{
    public class Launcher : ILauncher
    {
        private class Window : GameWindow, IOpenGlScenePresenterWindow
        {
            private readonly IContext _context;
            private readonly IOpenTkKeyBuffer _openTkKeyBuffer;
            private readonly IOpenGlScenePresenter _presenter;

            public Window(
                IComposerProxy composerProxy,
                IContext context,
                IOpenTkKeyBuffer openTkKeyBuffer)
            {
                _context = context;
                _openTkKeyBuffer = openTkKeyBuffer;
                _presenter = new OpenGlScenePresenter(() => composerProxy.SceneComposer, () => this);
                _context.Start();
            }

            public void SetSize(int width, int height)
            {
                Width = width;
                Height = height;
                _presenter.UpdateViewport();
            }

            protected override void OnKeyDown(KeyboardKeyEventArgs e)
            {
                _openTkKeyBuffer.Press(e);
                base.OnKeyDown(e);
            }

            protected override void OnUpdateFrame(FrameEventArgs e)
            {
                _context.ExecuteOnce();
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
        private readonly IComposerProxy _composerProxy;
        private readonly IOpenTkKeyBuffer _keyboard;
        private readonly IContextFactory _contextFactory;

        public Launcher(
            IComposerProxy composerProxy,
            IOpenTkKeyBuffer keyboard,
            IContextFactory contextFactory)
        {
            _composerProxy = composerProxy;
            _keyboard = keyboard;
            _contextFactory = contextFactory;

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

        public void Launch(ContextEngine contextEngine, IFileSystem fileSystem, IConfig config)
        {
            var context = _contextFactory.Create(contextEngine, fileSystem, config);
            _window = _window ?? new Window(_composerProxy, context, _keyboard);
            _window.Run();
            context.Stop();
        }
    }
}
