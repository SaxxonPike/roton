using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface ISoundBufferList : IList<int>
    {
        void Enqueue(ISound sound, int? offset = null, int? length = null);
        ISoundNote Dequeue();
    }
}