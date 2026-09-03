namespace Roton.Emulation.Data;

public interface ITimer
{
    ref Word Ticks { get; }
}