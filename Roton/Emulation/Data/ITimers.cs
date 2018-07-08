namespace Roton.Emulation.Data
{
    public interface ITimers
    {
        ITimer Player { get; }
        ITimer TimeLimit { get; }
    }
}