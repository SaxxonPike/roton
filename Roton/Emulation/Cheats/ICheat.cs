namespace Roton.Emulation.Cheats
{
    public interface ICheat
    {
        string Name { get; }
        void Execute();
    }
}