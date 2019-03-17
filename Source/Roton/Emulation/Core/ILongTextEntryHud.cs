namespace Roton.Emulation.Core
{
    public interface ILongTextEntryHud
    {
        string Show(string title, int x, int y, int maxLength, int textColor, int pipColor);
    }
}