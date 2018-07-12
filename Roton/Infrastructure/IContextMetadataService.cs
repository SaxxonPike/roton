using System;
using System.Collections.Generic;

namespace Roton.Infrastructure
{
    public interface IContextMetadataService
    {
        IEnumerable<ContextEngineAttribute> GetMetadata(object obj);
        IEnumerable<Type> GetTypes();
    }
}