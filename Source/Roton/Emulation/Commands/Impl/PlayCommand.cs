using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "PLAY")]
[Context(Context.Super, "PLAY")]
public sealed class PlayCommand(
    IEngineAccessor engine,
    IMusicEncoder musicEncoder,
    IParser parser,
    ISoundUnit soundUnit)
    : ICommand
{
    private IEngine Engine => engine.Instance;

    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];

        var notes = parser.ReadLine(context.Index, ref instruction, buffer);
        var sound = musicEncoder.Encode(notes);
        soundUnit.PlaySound(-1, sound);
        context.NextLine = false;
    }
}