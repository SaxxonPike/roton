using System.Collections.Generic;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Data.Impl
{
    [ContextEngine(ContextEngine.Original)]
    [ContextEngine(ContextEngine.Super)]
    public sealed class Boards : List<IPackedBoard>, IBoards
    {
    }
}