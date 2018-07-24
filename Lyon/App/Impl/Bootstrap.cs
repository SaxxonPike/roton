using System;
using System.IO;
using Roton.Emulation.Data.Impl;

namespace Lyon.App.Impl
{
    public class Bootstrap : IBootstrap
    {
        private readonly ILauncher _launcher;
        private readonly IContextFactory _contextFactory;

        public Bootstrap(ILauncher launcher, IContextFactory contextFactory)
        {
            _launcher = launcher;
            _contextFactory = contextFactory;
        }

        public void Boot(string[] args)
        {
            var fileName = args.Length > 0
                ? args[0]
                : null;

            var config = new Config
            {
                DefaultWorld = Path.GetFileNameWithoutExtension(fileName),
                RandomSeed = null,
                HomePath = fileName != null ? Path.GetDirectoryName(fileName) : Environment.CurrentDirectory,
                AudioDrumRate = 64,
                AudioSampleRate = 44100
            };

            if (fileName == null || fileName.EndsWith(".zzt", StringComparison.OrdinalIgnoreCase))
                _launcher.Launch(_contextFactory.Create(ContextEngine.Original, config));

            else if (fileName.EndsWith(".szt", StringComparison.OrdinalIgnoreCase))
                _launcher.Launch(_contextFactory.Create(ContextEngine.Super, config));
        }
    }
}