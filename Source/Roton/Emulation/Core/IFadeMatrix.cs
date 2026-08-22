using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IFadeMatrix
{
    int Width { get; }
    int Height { get; }
    int Left { get; }
    int Top { get; }

    void Randomize();
    void FadeOut(AnsiChar ac);
    void FadeIn();
}