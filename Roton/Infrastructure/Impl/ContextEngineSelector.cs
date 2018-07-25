using System;
using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure.Impl
{
    [ContextEngine(ContextEngine.Startup)]
    public class ContextEngineSelector : IContextEngineSelector
    {
        public ContextEngine Get(string filename)
        {
            if (filename == null || filename.EndsWith(".zzt", StringComparison.OrdinalIgnoreCase))
                return ContextEngine.Original;
            if (filename.EndsWith(".szt", StringComparison.OrdinalIgnoreCase))
                return ContextEngine.Super;
            throw new Exception("Unrecognized file extension");
        }
    }
}