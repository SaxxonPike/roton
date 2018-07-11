using System;
using System.Collections.Generic;

namespace Roton.Infrastructure
{
    public interface IContextMetadataService
    {
        IList<ContextEngineAttribute> GetMetadata(object obj);
        IList<Type> GetTypes();
    }
}