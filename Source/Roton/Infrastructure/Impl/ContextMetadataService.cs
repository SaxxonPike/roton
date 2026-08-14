using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure.Impl;

public abstract class ContextMetadataService(Context context) : IContextMetadataService
{
    public IEnumerable<ContextAttribute> GetMetadata(object obj) => obj
        .GetType()
        .GetCustomAttributes(true)
        .OfType<ContextAttribute>()
        .Where(a => a.Context == context)
        .ToList();

    public IEnumerable<Type> GetTypes() => GetType()
        .Assembly
        .GetTypes()
        .Where(t => t
            .GetCustomAttributes(true)
            .OfType<ContextAttribute>()
            .Any(a => a.Context == context))
        .ToList();
}