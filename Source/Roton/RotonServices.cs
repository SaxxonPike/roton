using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton;

/// <summary>
/// Maps services for dependency injection.
/// </summary>
public static class RotonServices
{
    /// <summary>
    /// Gets a map of services for use with dependency injection features.
    /// </summary>
    /// <param name="context">
    /// Game context type to get services for.
    /// </param>
    /// <param name="additionalAssemblies">
    /// Additional assemblies to scan.
    /// </param>
    /// <returns>
    /// All discovered types decorated with <see cref="ContextAttribute"/> that are applicable to the specified
    /// game context type.
    /// </returns>
    public static IEnumerable<RotonService> Get(Context context, params Assembly[] additionalAssemblies)
    {
        // Always include Startup.
        var contexts = ((IEnumerable<Context>)[Context.Startup, context]).Distinct();

        // Always include Roton's own assembly.
        var assemblies = additionalAssemblies.Concat([typeof(RotonServices).Assembly]).Distinct();

        // Fetch all types decorated with ContextAttribute.
        var metadataFactory = new ContextMetadataServiceFactory();
        var types = contexts.SelectMany(c => assemblies.SelectMany(a => metadataFactory.Get(c).GetTypes(a)));

        // Convert them to a RotonService map.
        var registrations = types.SelectMany(tc =>
        {
            var interfaces = tc.GetInterfaces()
                .Where(ti => ti != typeof(IDisposable));

            return interfaces.Select(ti => new RotonService(ti, tc));
        }).ToList();
        
        return registrations;
    }
}