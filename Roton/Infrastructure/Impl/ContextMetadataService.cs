using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure.Impl
{
    public abstract class ContextMetadataService : IContextMetadataService
    {
        private readonly Context _context;

        protected ContextMetadataService(Context context)
        {
            _context = context;
        }

        public IEnumerable<ContextAttribute> GetMetadata(object obj) => obj
            .GetType()
            .GetCustomAttributes(true)
            .OfType<ContextAttribute>()
            .Where(a => a.Context == _context)
            .ToList();

        public IEnumerable<Type> GetTypes() => GetType()
            .Assembly
            .GetTypes()
            .Where(t => t
                .GetCustomAttributes(true)
                .OfType<ContextAttribute>()
                .Any(a => a.Context == _context))
            .ToList();
    }
}