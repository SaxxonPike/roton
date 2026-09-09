using System;
using System.Diagnostics;
using Lyon;
using Lyon.App;
using Lyon.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Roton;
using Roton.Emulation.Core;
using Roton.Infrastructure;

// Create the DI container.
var services = new ServiceCollection();

services
    .AddLyonConfig(args, out var config, out var fileName);

// Determine which engine to use based on the world file name extension.
if (!ContextSelector.TryGetForWorldFileName(fileName, out var contextEngine))
    contextEngine = Context.Original;

services
    .AddRoton(Context.Ui, [typeof(Program).Assembly])
    .AddRoton(contextEngine)
    .AddLyonCommon()
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