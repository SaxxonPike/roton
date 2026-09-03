using System.Linq;
using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Statistics(
    IFacts facts,
    ICodeHeap heap,
    IBoardList boards)
    : IStatistics
{
    public int CalculateMemoryUsage() => 
        facts.BaseMemoryUsage + heap.Size + boards.Sum(b => b.Data.Length);

}