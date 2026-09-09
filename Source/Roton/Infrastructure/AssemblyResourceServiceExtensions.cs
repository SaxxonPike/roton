using Roton.Emulation.Core;

namespace Roton.Infrastructure;

public static class AssemblyResourceServiceExtensions
{
    public static IResource GetFromAssemblyOf<T>(this IAssemblyResourceService assemblyResourceService) =>
        assemblyResourceService.GetFromAssembly(typeof(T).Assembly);
}