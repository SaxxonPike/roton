namespace Roton.Core
{
    public interface IKeyboard
    {
        bool Alt { get; }
        bool Control { get; }
        bool Shift { get; }
        void Clear();
        int GetKey();
    }
}