using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ITransactor
{
    /// <remarks>
    /// RoZ: OopExecute (#GIVE/#TAKE)
    /// </remarks>
    bool Execute(ref OopContext context, ref Word instruction, bool take);
}