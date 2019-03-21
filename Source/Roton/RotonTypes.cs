using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton
{
    public static class RotonTypes
    {
        /// <summary>
        /// Get the complete type map for a context type.
        /// </summary>
        public static Type[] GetTypes(Context context)
        {
            return typeof(RotonTypes)
                .Assembly
                .ExportedTypes
                .Where(t => t.IsClass &&
                            !t.IsAbstract &&
                            t.GetCustomAttributes<ContextAttribute>()
                                .Any(c => c.Context == context))
                .ToArray();
        }

        /// <summary>
        /// Get context metadata for an object. 
        /// </summary>
        public static IReadOnlyList<ContextAttribute> GetMetadata(Context context, object obj)
        {
            return obj
                .GetType()
                .GetCustomAttributes(true)
                .OfType<ContextAttribute>()
                .Where(a => a.Context == context)
                .ToList();
        }
    }
}