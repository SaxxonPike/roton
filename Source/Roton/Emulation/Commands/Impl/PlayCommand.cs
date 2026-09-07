using System;
using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Commands.Impl;

[Context(Context.Original, "PLAY")]
[Context(Context.Super, "PLAY")]
internal sealed class PlayCommand(
    IMusicEncoder musicEncoder,
    IParser parser,
    ISoundPlayer soundPlayer)
    : ICommand
{
    public void Execute(ref OopContext context, ref Word instruction)
    {
        Span<char> buffer = stackalloc char[byte.MaxValue];

        var notes = parser.ReadLine(context.Index, ref instruction, buffer);
        using var sound = musicEncoder.Encode(notes);
        soundPlayer.PlaySound(-1, sound.Span);
        context.NextLine = false;
    }
}