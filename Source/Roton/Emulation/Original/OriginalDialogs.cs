using Roton.Emulation.Core;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalDialogs(
    IHud hud,
    IHighScoreListFactory highScoreListFactory)
    : IDialogs
{
    public void ShowAbout() =>
        hud.ShowHelp("About Roton...", "ABOUT");

    public void ShowHelp() =>
        hud.ShowHelp("Playing Roton", "GAME");

    public void ShowHighScores()
    {
        var list = highScoreListFactory.Load();
        hud.ShowHighScores(list);
    }
}