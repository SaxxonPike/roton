using System.Collections.Generic;

namespace Roton.Emulation.Data;

public interface IActorList : IEnumerable<IActor>
{
    int Capacity { get; }
    int Count { get; }
    IActor this[int index] { get; }
    IActor Player { get; }
    IActor ActorAt(Location location);
    int ActorIndexAt(Location location);
}