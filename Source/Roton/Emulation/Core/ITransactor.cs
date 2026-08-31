using Roton.Emulation.Data;

namespace Roton.Emulation.Core;

public interface ITransactor
{
    bool Execute(ref OopContext context, ref Word instruction, bool take);
}