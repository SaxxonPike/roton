using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IMessenger
{
    void SetMessage(int duration, IMessage message);
}