using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lyon;
using Lyon.App;
using Microsoft.Extensions.DependencyInjection;
using Roton;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

// Process command line arguments.
var fileName = args.TakeWhile(s => !s.StartsWith("--")).FirstOrDefault();
var switches = args.SkipWhile(s => !s.StartsWith("--")).Select(s => s.ToLower()).ToArray();

// Process configuration.
var config = new Config
{
    DefaultWorld = Path.GetFileNameWithoutExtension(fileName),
    RandomSeed = null,
    HomePath = fileName != null ? Path.GetDirectoryName(fileName) : Environment.CurrentDirectory,
    AudioDrumRate = 64,
    AudioSampleRate = 44100,
    AudioBufferSize = 2048,
    VideoScaleX = 2,
    VideoScaleY = 2,
    MasterClockNumerator = 100,
    MasterClockDenominator = 7275,
    FastMode = switches.Contains("--fast"),
    TraceOop = switches.Contains("--trace"),
    NoPesterMode = switches.Contains("--no-pester"),
    JoystickDeadZone = 0.3f
};

fileName ??= "TOWN.ZZT";

// Determine which engine to use based on the world file name extension.
var selector = new ContextEngineSelector();
if (!selector.TryGetForWorldFileName(fileName, out var contextEngine))
    throw new LyonException($"Cannot determine the format of the world file: {fileName}");

// Games in the Super engine look a little nicer with slightly taller graphics.
if (contextEngine == Context.Super)
    config.VideoScaleY *= 1.25f;

// Create the DI container.
var services = new ServiceCollection();
services.AddRoton(contextEngine, typeof(ILauncher).Assembly);
services.AddLyon(args, config);

// Build the container and run the app.
try
{
    using var container = services.BuildServiceProvider();

    if (config.TraceOop)
        container
            .GetService<ITracer>()?
            .Attach(Console.Out);

    container
        .GetRequiredService<ILauncher>()
        .Launch(container.GetRequiredService<IEngine>());
}
catch (Exception e)
{
    if (Debugger.IsAttached)
        throw;
    Console.WriteLine(e);
}