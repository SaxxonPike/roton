using System;
using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure.Impl
{
    [Context(Context.Startup)]
    public class ContextEngineSelector : IContextEngineSelector
    {
        public Context Get(string filename)
        {
            if (filename == null || filename.EndsWith(".zzt", StringComparison.OrdinalIgnoreCase))
                return Context.Original;
            if (filename.EndsWith(".szt", StringComparison.OrdinalIgnoreCase))
                return Context.Super;
            throw new Exception("Unrecognized file extension");
        }
    }
}