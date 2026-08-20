using System;
using System.Collections.Generic;
using System.Reflection;

namespace Roton.Infrastructure;

public interface IContextMetadataService
{
    IEnumerable<ContextAttribute> GetMetadata(object obj);
    IEnumerable<Type> GetTypes(Assembly assembly);
}