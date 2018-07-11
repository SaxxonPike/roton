using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl
{
    [ContextEngine(ContextEngine.Zzt)]
    [ContextEngine(ContextEngine.SuperZzt)]
    public sealed class Boards : List<IPackedBoard>, IBoards
    {
    }
}