using System;
using Lyon.Presenters;
using Roton.Emulation.Core;

namespace Lyon.App.Impl
{
    public class Launcher : ILauncher
    {
        private readonly Lazy<IWindow> _window;
        private readonly Lazy<IAudioPresenter> _audioPresenter;

        public Launcher(
            Lazy<IWindow> window,
            Lazy<IAudioPresenter> audioPresenter)
        {
            _window = window;
            _audioPresenter = audioPresenter;
        }

        private IWindow Window => _window.Value;
        private IAudioPresenter AudioPresenter => _audioPresenter.Value;

        public void Launch(IContext context)
        {
            AudioPresenter.Start();
            context.Start();
            Window.Start(14);
            context.Stop();
            AudioPresenter.Stop();
        }
    }
}