namespace Roton.Emulation.Infrastructure;

/// <summary>
/// Allows for lazy evaluation of a service.
/// </summary>
/// <typeparam name="T">
/// Type of service.
/// </typeparam>
public interface IDeferred<out T>
{
    /// <summary>
    /// Gets the service instance.
    /// </summary>
    T Instance { get; }
}