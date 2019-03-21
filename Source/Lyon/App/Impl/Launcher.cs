﻿using System;
using Lyon.Autofac;
using Lyon.Presenters;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Lyon.App.Impl
{
    [Service]
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
            engine.Start();
            Window.Start(14);
            engine.Stop();
            AudioPresenter.Stop();
        }
    }
}