using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Fader(
    IHud hud,
    IFacts facts)
    : IFader
{
    public void FadePurple()
    {
        hud.FadeBoard(facts.FadeTile);
        hud.RedrawBoard();
    }
}