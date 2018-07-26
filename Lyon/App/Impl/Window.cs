using System;
using DotSDL.Events;
using DotSDL.Graphics;
using Lyon.Presenters;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Lyon.App.Impl
{
    [ContextEngine(ContextEngine.Startup)]
    public class Window : SdlWindow, IWindow
    {
        private readonly Lazy<IKeyboardPresenter> _keyboardPresenter;
        private readonly Lazy<IScenePresenter> _scenePresenter;
        private bool _closed;

        public int Width => WindowWidth;
        public int Height => WindowHeight;
        
        private IKeyboardPresenter KeyboardPresenter => _keyboardPresenter.Value;
        private IScenePresenter ScenePresenter => _scenePresenter.Value;

        public Window(
            IConfig config,
            Lazy<IKeyboardPresenter> keyboardPresenter,
            Lazy<IScenePresenter> scenePresenter) : base("Roton",
            new Point {X = WindowPosUndefined, Y = WindowPosUndefined},
            640 * config.VideoScale, 350 * config.VideoScale,
            640, 350)
        {
            _keyboardPresenter = keyboardPresenter;
            _scenePresenter = scenePresenter;
            KeyPressed += OnKeyDown;
            Closed += OnClosed;
        }

        private void OnClosed(object sender, WindowEvent e)
        {
            Close();
        }

        public void SetSize(int width, int height)
        {
            ScenePresenter.UpdateViewport();
        }

        public void Start(int updateRate)
        {
            base.Start((uint) updateRate);
        }

        public void Close()
        {
            if (_closed)
                return;
            
            _closed = true;
            Stop();
        }

        private void OnKeyDown(object obj, KeyboardEvent e)
        {
            KeyboardPresenter.Press(e);
        }

        public override IntPtr GetCanvasPointer()
        {
            var bitmap = ScenePresenter.Render();
            return bitmap.Bits.Length == 0 ? IntPtr.Zero : bitmap.BitsPointer;
        }

        protected override void OnUpdate()
        {
        }

        public void MakeCurrent()
        {
        }

        public void SwapBuffers()
        {
        }
    }
}