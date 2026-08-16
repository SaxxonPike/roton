using System;

namespace Roton.Infrastructure.Impl;

public sealed class LazyResolver<T>(IServiceProvider serviceProvider) : ILazy<T>
{
    private readonly Lazy<T> _lazy = new(() => (T)serviceProvider.GetService(typeof(T)));

    public T Value => _lazy.Value;
}