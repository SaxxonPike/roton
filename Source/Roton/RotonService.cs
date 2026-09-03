using System;

namespace Roton;

/// <summary>
/// Describes a service type and its implementation for use with dependency injection features.
/// </summary>
/// <param name="service">
/// Service type.
/// </param>
/// <param name="implementation">
/// Concrete type of the service.
/// </param>
public readonly struct RotonService(Type service, Type implementation)
{
    /// <summary>
    /// Service type.
    /// </summary>
    public Type Service { get; } = service;

    /// <summary>
    /// Concrete type of the service.
    /// </summary>
    public Type Implementation { get; } = implementation;

    public override string ToString() => 
        $"{Service} <- {Implementation}";
}