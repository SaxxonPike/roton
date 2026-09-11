using Roton.Emulation.Core;

namespace Roton.Infrastructure;

internal static class AssemblyResourceServiceExtensions
{
    public static IResource GetFromAssemblyOf<T>(this IAssemblyResourceService assemblyResourceService) =>
        assemblyResourceService.GetFromAssembly(typeof(T).Assembly);
}