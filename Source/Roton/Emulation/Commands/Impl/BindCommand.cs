using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "BIND")]
[Context(Context.Super, "BIND")]
public sealed class BindCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(IOopContext context)
    {
        var search = new SearchContext();
        var target = Engine.Parser.ReadWord(context.Index, context);
        if (Engine.Parser.GetTarget(context.Index, search, target))
        {
            var targetActor = Engine.Actors[search.SearchIndex];
            context.Actor.Pointer = targetActor.Pointer;
            context.Actor.Length = targetActor.Length;
            context.Instruction = 0;
        }
    }
}