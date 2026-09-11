using System.Collections.Generic;
using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface IBoardList : IEnumerable<IPackedBoard>
{
    IPackedBoard this[int index] { get; set; }
    int Count { get; }
    void Add(IPackedBoard board);
    bool Remove(IPackedBoard board);
    void Clear();
}