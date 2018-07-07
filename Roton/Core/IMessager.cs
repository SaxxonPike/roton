namespace Roton.Core
{
    public interface IMessager
    {
        void RaiseError(string error);
        void SetMessage(int duration, IMessage message);
    }
}