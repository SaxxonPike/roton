using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalFileTitles : IFileTitles
{
    public string? GetTitle(string? fileName) =>
        fileName?.ToUpperInvariant() switch
        {
            "TOWN" =>
                "TOWN       The Town of ZZT",
            "DEMO" =>
                "DEMO       Demo of the ZZT World Editor",
            "PHYSICS" =>
                "PHYSICS    The Physics Behind ZZT",
            "TOUR" =>
                "TOUR       Guided Tour ZZT's Other Worlds",
            "CAVES" =>
                "CAVES      The Caves of ZZT",
            "CITY" =>
                "CITY       Underground City of ZZT",
            "DUNGEONS" =>
                "DUNGEONS   The Dungeons of ZZT",
            "BEST" =>
                "BEST       The Best of ZZT",
            _ => null
        };
}