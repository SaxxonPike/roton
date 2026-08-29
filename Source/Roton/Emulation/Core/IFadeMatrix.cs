using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IFadeMatrix
{
    void Randomize();
    void FadeOut(AnsiChar ac);
    void FadeIn();
}