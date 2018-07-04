using System.Collections.Generic;

namespace Roton.Core
{
    public interface IBoardList : IEnumerable<IPackedBoard>
    {
        IPackedBoard this[int index] { get; set; }
        int Count { get; }
        void Add(IPackedBoard board);
        bool Remove(IPackedBoard board);
        void Clear();
    }
}