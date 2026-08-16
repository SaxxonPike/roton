using System;
using System.Collections.Generic;
using System.Reflection;
using Roton.Infrastructure.Impl;

namespace Roton.Infrastructure;

public interface IContextMetadataService
{
    IEnumerable<ContextAttribute> GetMetadata(object obj);
    IEnumerable<Type> GetTypes(Assembly assembly);
}