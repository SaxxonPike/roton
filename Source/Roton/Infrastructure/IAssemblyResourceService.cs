using Roton.Emulation.Core;

namespace Roton.Infrastructure;

public interface IAssemblyResourceService
{
    IResource GetFromAssemblyOf<T>();
}