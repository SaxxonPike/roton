using System;
using Microsoft.Extensions.DependencyInjection;
using Roton.Infrastructure;

namespace Roton.Test.Infrastructure;

public class TestFactory(IServiceProvider serviceProvider) : IFactory
{
    public T Create<T>(params object?[] args) =>
        ActivatorUtilities.CreateInstance<T>(serviceProvider, args);
}