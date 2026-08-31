using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
public class SuperDialogs(
    IBroadcaster broadcaster,
    IFacts facts)
    : IDialogs
{
    public void ShowAbout()
    {
        // No-op in Super engine.
    }

    public void ShowHelp() =>
        broadcaster.BroadcastLabel(0, facts.HintLabel, false);
}