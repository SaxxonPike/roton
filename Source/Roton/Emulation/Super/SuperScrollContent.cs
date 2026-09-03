using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Super;

[Context(Context.Super)]
internal sealed class SuperScrollContent : ScrollContent
{
    public override int LineWidth => 60;
}