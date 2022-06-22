using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public sealed class OriginalWorldTitleService : IWorldTitleService
{
    public string GetTitle(string fileName)
    {
        return fileName.ToUpperInvariant() switch
        {
            "TOWN" => "The Town of ZZT",
            "DEMO" => "Demo of the ZZT World Editor",
            "PHYSICS" => "The Physics Behind ZZT",
            "TOUR" => "Guided Tour ZZT's Other Worlds",
            "CAVES" => "The Caves of ZZT",
            "CITY" => "Underground City of ZZT",
            "DUNGEONS" => "The Dungeons of ZZT",
            _ => null
        };
    }
}