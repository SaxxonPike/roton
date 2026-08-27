using Roton.Emulation.Data.Impl;
using Roton.Infrastructure;

namespace Roton.Emulation.Original;

[Context(Context.Original)]
public class OriginalScrollContent : ScrollContent
{
    public override int LineWidth => 50;
}