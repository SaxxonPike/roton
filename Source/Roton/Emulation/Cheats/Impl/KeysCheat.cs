using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Cheats.Impl;

/// <summary>
/// Represents the "KEYS" cheat, which grants all keys to the player.
/// </summary>
[Context(Context.Original, "KEYS")]
[Context(Context.Super, "KEYS")]
internal sealed class KeysCheat(
    IKeyList keyList)
    : ICheat
{
    public void Execute(bool clear)
    {
        for (var i = 0; i < 7; i++)
            keyList[i] = true;
    }
}