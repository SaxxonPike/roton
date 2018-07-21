using System;
using System.IO;
using Roton.Emulation.Data.Impl;

namespace Lyon.App
{
    public class Bootstrap : IBootstrap
    {
        private readonly ILauncher _launcher;

        public Bootstrap(ILauncher launcher)
        {
            _launcher = launcher;
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
                HomePath = fileName != null ? Path.GetDirectoryName(fileName) : Environment.CurrentDirectory
            };

            if (fileName == null || fileName.EndsWith(".zzt", StringComparison.OrdinalIgnoreCase))
                _launcher.Launch(ContextEngine.Original, config);

            else if (fileName.EndsWith(".szt", StringComparison.OrdinalIgnoreCase))
                _launcher.Launch(ContextEngine.Super, config);
        }
    }
}