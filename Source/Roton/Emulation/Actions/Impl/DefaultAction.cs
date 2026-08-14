using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public sealed class DefaultAction : IAction
{
    public void Act(int index)
    {
    }
}