using System;
using Lyon.Presenters;
using OpenTK;
using OpenTK.Input;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Interface.Events;
using Roton.Interface.Video;

namespace Lyon.App
{
    public class Launcher : ILauncher
    {
        private class Window : GameWindow, IOpenGlScenePresenterWindow
        {
            private readonly IContext _context;
            private readonly IKeyboardPresenter _keyboardPresenter;
            private readonly IScenePresenter _scenePresenter;

            public Window(
                IComposerProxy composerProxy,
                IContext context,
                IKeyboardPresenter keyboardPresenter)
            {
                _context = context;
                _keyboardPresenter = keyboardPresenter;
                _scenePresenter = new ScenePresenter(() => composerProxy.SceneComposer, () => this);
                _context.Start();
            }

            public void SetSize(int width, int height)
            {
                Width = width;
                Height = height;
                _scenePresenter.UpdateViewport();
            }

            protected override void OnKeyDown(KeyboardKeyEventArgs e)
            {
                _keyboardPresenter.Press(e);
                base.OnKeyDown(e);
            }

            protected override void OnUpdateFrame(FrameEventArgs e)
            {
                _context.ExecuteOnce();
                base.OnUpdateFrame(e);
            }

            protected override void OnRenderFrame(FrameEventArgs e)
            {
                _scenePresenter.Render();
                base.OnRenderFrame(e);
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                _scenePresenter.UpdateViewport();
            }
        }

        private Window _window;
        private readonly IComposerProxy _composerProxy;
        private readonly IKeyboardPresenter _keyboardPresenter;
        private readonly IContextFactory _contextFactory;

        public Launcher(
            IComposerProxy composerProxy,
            IKeyboardPresenter keyboardPresenter,
            IContextFactory contextFactory)
        {
            _composerProxy = composerProxy;
            _keyboardPresenter = keyboardPresenter;
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

        public void Launch(ContextEngine contextEngine, IConfig config)
        {
            using (var audioPresenter = new AudioPresenter(() => _composerProxy.AudioComposer))
            {
                var context = _contextFactory.Create(contextEngine, config);
                
                _composerProxy.SetSound(context.Engine.DrumBank, 44100, 64);
                audioPresenter.Start();
                
                _window = _window ?? new Window(_composerProxy, context, _keyboardPresenter);
                _window.Run();
                context.Stop();                
            }
        }
    }
}
