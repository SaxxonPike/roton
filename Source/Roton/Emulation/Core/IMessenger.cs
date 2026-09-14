using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IMessenger
{
    /// <remarks>
    /// RoZ: DisplayMessage
    /// </remarks>
    void SetMessage(int duration, IReadOnlyList<string> message);
}