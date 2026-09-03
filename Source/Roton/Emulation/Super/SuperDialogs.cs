using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperDialogs(
    IBroadcaster broadcaster,
    IFacts facts,
    IHighScoreListFactory highScoreListFactory,
    IHighScoreHud highScoreHud)
    : IDialogs
{
    public void ShowAbout()
    {
        // No-op in Super engine.
    }

    public void ShowHelp() =>
        broadcaster.BroadcastLabel(0, facts.HintLabel, false);
    
    public void ShowHighScores()
    {
        var list = highScoreListFactory.Load();
        highScoreHud.ShowHighScores(list);
    }
}