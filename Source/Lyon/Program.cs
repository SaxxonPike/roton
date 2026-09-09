using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lyon;
using Lyon.App;
using Lyon.Common;
using Lyon.Common.App;
using Lyon.Common.App.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Roton;
using Roton.Emulation.Core;
using Roton.Infrastructure;

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

// Determine which engine to use based on the world file name extension.
if (!ContextSelector.TryGetForWorldFileName(fileName, out var contextEngine))
    throw new LyonException($"Cannot determine the format of the world file: {fileName}");

// Create the DI container.
var services = new ServiceCollection();

services
    .AddLyonConfig(args, out var config);

services
    .AddRoton(Context.Ui, [typeof(Program).Assembly])
    .AddRoton(contextEngine)
    .AddLyonCommon(args)
    .AddLyon();

// Build the container and run the app.
try
{
    using var container = services.BuildServiceProvider();

    if (config.GetValue<bool?>("Roton:Engine:TraceOop") == true)
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