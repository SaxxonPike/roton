using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "BIND")]
[Context(Context.Super, "BIND")]
public sealed class BindCommand : ICommand
{
    private readonly Lazy<IEngine> _engine;
    private IEngine Engine => _engine.Value;

    public BindCommand(Lazy<IEngine> engine)
    {
        _engine = engine;
    }

    public void Execute(IOopContext context)
    {
        var search = new SearchContext();
        var target = Engine.Parser.ReadWord(context.Index, context);
        search.SearchTarget = target;
        search.Index = context.Index;
        if (Engine.Parser.GetTarget(search))
        {
            var targetActor = Engine.Actors[search.SearchIndex];
            context.Actor.Pointer = targetActor.Pointer;
            context.Actor.Length = targetActor.Length;
            context.Instruction = 0;
        }
    }
}