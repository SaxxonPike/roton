using System.Collections.Generic;
using Roton.Emulation.Data.Impl;

namespace Roton.Emulation.Data;

public interface IActors : IEnumerable<IActor>
{
    int Capacity { get; }
    int Count { get; }
    IActor this[int index] { get; }
    IActor Player { get; }
    IActor ActorAt(Location location);
    int ActorIndexAt(Location location);
}