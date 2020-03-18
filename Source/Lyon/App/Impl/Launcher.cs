using System;
using System.Diagnostics;
using System.IO;
using Lyon.Presenters;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Lyon.App.Impl
{
    [Context(Context.Startup)]
    public sealed class Launcher : ILauncher
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

        private void OnExited(object sender, EventArgs e)
        {
            Window.Close();
        }

        public void Launch(IEngine engine)
        {
            AudioPresenter.Start();
            engine.Exited += OnExited;
            using var traceLog = new FileStream("trace.log", FileMode.Create);
            using var traceWriter = new StreamWriter(traceLog);
            engine.Tracer.Attach(traceWriter);
            engine.Start();
            Window.Start(72.75f);
            engine.Stop();
            engine.Tracer.Detach(traceWriter);
            AudioPresenter.Stop();
        }
    }
}