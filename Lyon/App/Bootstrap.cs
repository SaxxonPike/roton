﻿using System;
using System.IO;
using Roton.Emulation.Data.Impl;

namespace Lyon.App
{
    public class Bootstrap : IBootstrap
    {
        private readonly ILauncher _launcher;
        private readonly IFileSystemFactory _fileSystemFactory;

        public Bootstrap(ILauncher launcher, IFileSystemFactory fileSystemFactory)
        {
            _launcher = launcher;
            _fileSystemFactory = fileSystemFactory;
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