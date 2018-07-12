using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface IMessenger
    {
        void RaiseError(string error);
        void SetMessage(int duration, IMessage message);
    }
}