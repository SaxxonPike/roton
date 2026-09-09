namespace Roton.Composers.Audio.Synths;

public interface ISynthInterpolator
{
    /// <summary>
    /// Applies interpolation.
    /// </summary>
    /// <param name="halfPhase">
    /// Half phase of the current sample.
    /// </param>
    /// <param name="halfPhasePerSample">
    /// Amount of phase advanced per sample.
    /// </param>
    float PolyBlep(float halfPhase, float halfPhasePerSample);
}