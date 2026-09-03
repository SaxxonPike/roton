namespace Roton.Emulation.Core;

public interface IDamager
{
    /// <remarks>
    /// RoZ: BoardDamageTile
    /// </remarks>
    void Harm(int index);
}