using System.Collections.Concurrent;
using System.Text;

namespace Roton.Emulation.Data.Impl;

public static class StringBuilderPool
{
    private static readonly ConcurrentQueue<StringBuilder> Pool = [];
    
    public static StringBuilder Rent()
    {
        if (Pool.TryDequeue(out var sb))
            return sb;
        return new StringBuilder();
    }
    
    public static void Return(StringBuilder sb)
    {
        sb.Clear();
        Pool.Enqueue(sb);
    }
}