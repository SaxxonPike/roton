using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class Boards : List<IPackedBoard>, IBoards;