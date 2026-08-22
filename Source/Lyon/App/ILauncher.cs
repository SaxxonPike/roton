using Roton.Emulation.Core;

namespace Lyon.App;

/// <summary>
/// Launches Lyon frontends over Roton backends.
/// </summary>
public interface ILauncher
{
    /// <summary>
    /// Start Lyon using the specified engine.
    /// </summary>
    void Launch(IEngine engine);
}