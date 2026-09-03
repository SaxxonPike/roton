using System.Collections.Generic;
using Roton.Infrastructure;

namespace Roton.Emulation.Data.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class BoardList : List<IPackedBoard>, IBoardList;