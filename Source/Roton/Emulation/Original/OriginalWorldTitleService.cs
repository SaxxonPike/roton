using Roton.Emulation.Core;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Original
{
    [Context(Context.Original)]
    public class OriginalWorldTitleService : IWorldTitleService
    {
        public string GetTitle(string fileName)
        {
            switch (fileName.ToUpperInvariant())
            {
                case "TOWN":
                    return "The Town of ZZT";
                case "DEMO":
                    return "Demo of the ZZT World Editor";
                case "PHYSICS":
                    return "The Physics Behind ZZT";
                case "TOUR":
                    return "Guided Tour ZZT's Other Worlds";
                case "CAVES":
                    return "The Caves of ZZT";
                case "CITY":
                    return "Underground City of ZZT";
                case "DUNGEONS":
                    return "The Dungeons of ZZT";
                default:
                    return null;
            }
        }
    }
}