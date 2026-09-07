using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IFadeMatrix
{
    /// <remarks>
    /// RoZ: GenerateTransitionTable
    /// </remarks>
    void Randomize();
    void FadeOut(AnsiChar ac);
    void FadeIn();
}