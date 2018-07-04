using System.Collections.Generic;

namespace Roton.Core
{
    public interface IActors : IEnumerable<IActor>
    {
        int Capacity { get; }
        int Count { get; }
        IActor this[int index] { get; }
        IActor Player { get; }
    }
}