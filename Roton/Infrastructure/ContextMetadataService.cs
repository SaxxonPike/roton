using System;
using System.Collections.Generic;
using System.Linq;
using Roton.Emulation.Data.Impl;

namespace Roton.Infrastructure
{
    public abstract class ContextMetadataService : IContextMetadataService
    {
        private readonly ContextEngine _contextEngine;

        protected ContextMetadataService(ContextEngine contextEngine)
        {
            _contextEngine = contextEngine;
        }

        public IList<ContextEngineAttribute> GetMetadata(object obj) => obj
            .GetType()
            .GetCustomAttributes(true)
            .OfType<ContextEngineAttribute>()
            .Where(a => a.ContextEngine == _contextEngine)
            .ToList();

        public IList<Type> GetTypes() => GetType()
            .Assembly
            .GetTypes()
            .Where(t => t
                .GetCustomAttributes(true)
                .OfType<ContextEngineAttribute>()
                .Any(a => a.ContextEngine == _contextEngine))
            .ToList();
    }
}