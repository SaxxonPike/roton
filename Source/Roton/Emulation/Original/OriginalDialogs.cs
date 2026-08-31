using Roton.Emulation.Core;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
internal sealed class OriginalDialogs(IHud hud)
    : IDialogs
{
    public void ShowAbout() =>
        hud.ShowHelp("About Roton...", "ABOUT");

    public void ShowHelp() =>
        hud.ShowHelp("Playing Roton", "GAME");
}