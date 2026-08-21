using System;
using System.Collections.Generic;

namespace Roton.Infrastructure.Impl;

[Context(Context.Startup)]
public sealed class ContextEngineSelector : IContextEngineSelector
{
    private static readonly Dictionary<string, Context> ContextMap = new()
    {
        { ".zzt", Context.Original },
        { ".szt", Context.Super }
    };

    public bool TryGetForWorldFileName(string filename, out Context context)
    {
        if (filename == null)
            throw new ArgumentNullException(nameof(filename));

        foreach (var kv in ContextMap)
        {
            if (!filename.EndsWith(kv.Key, StringComparison.OrdinalIgnoreCase))
                continue;

            context = kv.Value;
            return true;
        }

        context = default;
        return false;
    }
}