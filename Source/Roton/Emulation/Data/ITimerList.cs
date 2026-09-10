namespace Roton.Emulation.Data;

public interface ITimerList
{
    ITimer Player { get; }
    ITimer TimeLimit { get; }
}