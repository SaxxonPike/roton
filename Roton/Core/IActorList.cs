using System.Collections.Generic;

namespace Roton.Core
{
    public interface IActorList : IList<IActor>
    {
        int Capacity { get; }
    }
}
