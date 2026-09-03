using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperFileTitles : IFileTitles
{
    public string? GetTitle(string fileName) =>
        fileName.ToUpperInvariant() switch
        {
            "PROVING" =>
                "PROVING  ZZT's Proving Grounds",
            "FOREST" =>
                "FOREST   ZZT's Lost Forest",
            "MONSTER" =>
                "MONSTER  ZZT's Monster Zoo",
            _ => null
        };
}