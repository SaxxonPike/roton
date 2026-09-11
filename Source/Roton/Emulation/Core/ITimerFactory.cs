using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ITimerFactory
{
    ITimer Create(int offset);
}