using Microsoft.Extensions.DependencyInjection;
using Roton.Infrastructure;

namespace Lyon.Common.App.Impl;

public sealed class Factory(IServiceProvider serviceProvider) : IFactory
{
    public T Create<T>(params object?[] args) =>
        ActivatorUtilities.CreateInstance<T>(serviceProvider, args);
}