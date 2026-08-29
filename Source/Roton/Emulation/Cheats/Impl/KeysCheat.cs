using System;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

[Context(Context.Original, "KEYS")]
[Context(Context.Super, "KEYS")]
internal sealed class KeysCheat(
    IKeyList keyList)
    : ICheat
{
    public void Execute(ReadOnlySpan<char> name, bool clear)
    {
        for (var i = 0; i < 7; i++)
            keyList[i] = true;
    }
}