using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Lyon;
using Lyon.App;
using Microsoft.Extensions.DependencyInjection;
using Roton;
using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

// Process command line arguments. This (loosely) follows the "GetOpt" convention.

var switches = args
    .TakeWhile(s => s != "--")
    .Where(s => s.StartsWith("--") || s is ['-', _])
    .ToList();

var fileNames = args
    .TakeWhile(s => s != "--")
    .Where(s => !s.StartsWith("--") && s is not ['-', _])
    .Concat(args.SkipWhile(s => s != "--"))
    .DefaultIfEmpty("TOWN.ZZT")
    .ToList();

var fileName = fileNames.First();

// Process configuration.
var config = new Config
{
    DefaultWorld = Path.GetFileNameWithoutExtension(fileName),
    RandomSeed = null,
    HomePath = Path.GetDirectoryName(fileName),
    AudioDrumRate = 64,
    AudioSampleRate = 44100,
    AudioBufferSize = 2048,
    VideoScaleX = 2,
    VideoScaleY = 2,
    MasterClockNumerator = 100,
    MasterClockDenominator = 7275,
    FastMode = switches.Contains("--fast") || switches.Contains("-f"),
    TraceOop = switches.Contains("--trace") || switches.Contains("-t"),
    NoPesterMode = switches.Contains("--no-pester") || switches.Contains("-p"),
    JoystickDeadZone = 0.5f,
    JoystickDenoiseZone = 0.1f,
    DisableJoystick = switches.Contains("--no-joystick")
};

// Determine which engine to use based on the world file name extension.
if (!ContextSelector.TryGetForWorldFileName(fileName, out var contextEngine))
    throw new LyonException($"Cannot determine the format of the world file: {fileName}");

// Games in the Super engine look a little nicer with slightly taller graphics.
if (contextEngine == Context.Super)
    config.VideoScaleY *= 1.25f;

// Create the DI container.
var services = new ServiceCollection();
Assembly[] additionalAssemblies = [typeof(ILauncher).Assembly];
services.AddRoton(contextEngine, additionalAssemblies);
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
        .Launch();
}
catch (Exception e)
{
    if (Debugger.IsAttached)
        throw;
    Console.WriteLine(e);
}