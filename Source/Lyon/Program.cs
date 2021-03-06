﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Autofac;
using Lyon.App;
using Lyon.Autofac;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Lyon
{
    public static class Program
    {
        // STAThread is required for open/save dialogs.

        [STAThread]
        private static void Main(string[] args)
        {
            var fileName = args.TakeWhile(s => !s.StartsWith("--")).FirstOrDefault();
            var switches = args.SkipWhile(s => !s.StartsWith("--")).Select(s => s.ToLower()).ToArray();

            var config = new Config
            {
                DefaultWorld = Path.GetFileNameWithoutExtension(fileName),
                RandomSeed = null,
                HomePath = fileName != null ? Path.GetDirectoryName(fileName) : Environment.CurrentDirectory,
                AudioDrumRate = 64,
                AudioSampleRate = 44100,
                AudioBufferSize = 2048,
                VideoScale = 2,
                MasterClockNumerator = 100,
                MasterClockDenominator = 7275,
                FastMode = args.Contains("--fast")
            };

            var selector = new ContextEngineSelector();
            var contextEngine = selector.Get(fileName);

            var builder = new ContainerBuilder();

            builder.RegisterInstance(config)
                .As<IConfig>()
                .SingleInstance();

            builder.RegisterModule(new RotonModule(contextEngine));
            builder.RegisterModule(new LyonModule(args));

            try
            {
                using (var container = builder.Build())
                {
                    container
                        .Resolve<ILauncher>()
                        .Launch(container.Resolve<IEngine>());
                }
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached)
                    throw;
                Console.WriteLine(e);
            }
        }
    }
}