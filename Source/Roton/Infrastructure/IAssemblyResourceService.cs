using System.Reflection;
using Roton.Emulation.Core;

namespace Roton.Infrastructure;

public interface IAssemblyResourceService
{
    IResource GetFromAssembly(Assembly assembly);
}