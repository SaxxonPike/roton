using System;
using System.Collections.Generic;
using Roton.Infrastructure.Impl;

namespace Roton.Infrastructure
{
    public interface IContextMetadataService
    {
        IEnumerable<ContextAttribute> GetMetadata(object obj);
        IEnumerable<Type> GetTypes();
    }
}