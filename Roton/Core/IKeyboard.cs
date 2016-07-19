namespace Roton.Core
{
    public interface IKeyboard
    {
        bool Alt { get; }
        void Clear();
        bool Control { get; }
        int GetKey();
        bool Shift { get; }
    }
}