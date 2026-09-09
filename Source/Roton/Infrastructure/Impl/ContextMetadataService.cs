using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Roton.Infrastructure.Impl;

internal sealed class ContextMetadataService(Context context) : IContextMetadataService
{
    public IEnumerable<ContextAttribute> GetMetadata(object obj) =>
    [
        .. obj
            .GetType()
            .GetCustomAttributes(true)
            .OfType<ContextAttribute>()
            .Where(a => a.Context == context)
    ];

    public IEnumerable<Type> GetTypes(Assembly assembly) =>
    [
        .. assembly
            .GetTypes()
            .Where(t => t
                .GetCustomAttributes(true)
                .OfType<ContextAttribute>()
                .Any(a => a.Context == context))
    ];
}