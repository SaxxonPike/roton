using Roton.Infrastructure;

namespace Roton.Emulation.Actions.Impl;

/// <summary>
/// Represents the default tick action.
/// </summary>
[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class DefaultAction : IAction
{
    public void Act(int index)
    {
    }
}