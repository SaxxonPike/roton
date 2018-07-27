namespace Roton.Emulation.Cheats
{
    public interface ICheat
    {
        void Execute(string name, bool clear);
    }
}