using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        [DebuggerStepThrough]
        public IEnumerable<ContextAttribute> GetMetadata(object obj) => 
            RotonTypes.GetMetadata(_context, obj);

        [DebuggerStepThrough]
        public IEnumerable<Type> GetTypes() =>
            RotonTypes.GetTypes(_context);
    }
}