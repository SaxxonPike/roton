using System;

namespace Roton.Emulation.Infrastructure;

/// <inheritdoc />
/// <param name="serviceProvider">
/// Service provider that will be used to resolve the service.
/// </param>
internal sealed class Deferred<T>(IServiceProvider serviceProvider)
    : IDeferred<T>
{
    /// <summary>
    /// Holds the lazy service resolver.
    /// </summary>
    private readonly Lazy<T> _service = new(() => (T)serviceProvider.GetService(typeof(T)));

    /// <inheritdoc />
    public T Instance => _service.Value;
}