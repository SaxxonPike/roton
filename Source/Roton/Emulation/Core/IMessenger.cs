using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IMessenger
{
    /// <remarks>
    /// RoZ: DisplayMessage
    /// </remarks>
    void SetMessage(int duration, IMessage message);
}