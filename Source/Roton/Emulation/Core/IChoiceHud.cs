namespace Roton.Emulation.Core
{
    public interface IChoiceHud
    {
        int Show(bool performSelection, int x, int y, string message, int currentValue, string barText);
    }
}