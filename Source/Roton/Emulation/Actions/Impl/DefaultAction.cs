using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class DefaultAction : IAction
{
    public void Act(int index)
    {
    }
}