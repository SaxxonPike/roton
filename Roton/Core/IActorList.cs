using System.Collections.Generic;

namespace Roton.Core
{
    public interface IActorList : IEnumerable<IActor>
    {
        IActor this[int index] { get; }
        int Capacity { get; }
        int Count { get; }
    }
}