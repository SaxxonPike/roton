using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalDialogs(
    IHighScoreListFactory highScoreListFactory,
    IHighScoreHud highScoreHud,
    IScroll scroll)
    : IDialogs
{
    public void ShowAbout() =>
        scroll.ShowHelpFile("About Roton...", "ABOUT");

    public void ShowHelp() =>
        scroll.ShowHelpFile("Playing Roton", "GAME");

    public void ShowHighScores()
    {
        var list = highScoreListFactory.Load();
        highScoreHud.ShowHighScores(list);
    }
}