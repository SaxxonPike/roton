using Roton.Core;

namespace Roton.Emulation.Execution
{
    public interface ITimers
    {
        ITimer Player { get; }
        ITimer TimeLimit { get; }
    }
}