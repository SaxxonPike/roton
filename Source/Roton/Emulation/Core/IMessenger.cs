using System.Collections.Generic;

namespace Roton.Emulation.Core;

public interface IMessenger
{
    /// <remarks>
    /// RoZ: DisplayMessage
    /// </remarks>
    void SetMessage(int duration, IReadOnlyList<string> message);
}