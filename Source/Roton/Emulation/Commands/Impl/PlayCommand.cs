using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "PLAY")]
[Context(Context.Super, "PLAY")]
public sealed class PlayCommand(IEngineAccessor engine) : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        var notes = Engine.Parser.ReadLine(context.Index, ref instruction);
        var sound = Engine.MusicEncoder.Encode(notes);
        Engine.PlaySound(-1, sound);
        context.NextLine = false;
    }
}