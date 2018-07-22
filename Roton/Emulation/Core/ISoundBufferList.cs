using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core
{
    public interface ISoundBufferList : IList<int>
    {
        void Enqueue(ISound sound);
        ISoundNote Dequeue();
    }

    public interface ISoundNote
    {
        int Note { get; }
        int Duration { get; }
    }
    
    
}