namespace Roton.Emulation.Data;

public interface ITimerFactory
{
    ITimer Create(int offset);
}