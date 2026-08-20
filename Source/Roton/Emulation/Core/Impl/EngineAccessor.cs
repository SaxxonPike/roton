using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Startup)]
public sealed class EngineAccessor : IEngineAccessor
{
    public IEngine Instance { get; set; } = null!;
}