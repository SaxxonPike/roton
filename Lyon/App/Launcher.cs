using System;
using DotSDL.Events;
using DotSDL.Graphics;
using Lyon.Presenters;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Interface.Events;
using Roton.Interface.Video;

namespace Lyon.App
{
    public class Launcher : ILauncher
    {
        private class Window : SdlWindow, IScenePresenterWindow
        {
            public int Width => WindowWidth;
            public int Height => WindowHeight;
            
            private readonly IContext _context;
            private readonly IKeyboardPresenter _keyboardPresenter;
            private readonly IScenePresenter _scenePresenter;

            public Window(
                IComposerProxy     composerProxy,
                IContext           context,
                IKeyboardPresenter keyboardPresenter) : base("Roton",
                                                             new Point {X = WindowPosUndefined, Y = WindowPosUndefined},
                                                             640, 350)
            {
                _context = context;
                _keyboardPresenter = keyboardPresenter;
                _scenePresenter = new ScenePresenter(() => composerProxy.SceneComposer, () => this);
                _context.Start();

                KeyPressed += OnKeyDown;
            }

            public void SetSize(int width, int height)
            {
                _scenePresenter.UpdateViewport();
            }

            private void OnKeyDown(object obj, KeyboardEvent e)
            {
                _keyboardPresenter.Press(e);
            }

            public override IntPtr GetCanvasPointer()
            {
                var bitmap = _scenePresenter.Render();
                return bitmap.Bits.Length == 0 ? IntPtr.Zero : bitmap.BitsPointer;
            }

            protected override void OnUpdate()
            {
                _context.ExecuteOnce();
            }

            public void MakeCurrent() { }
            public void SwapBuffers() { }
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
                _window.Start(14);
                context.Stop();
            }
        }
    }
}
