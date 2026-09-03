using Roton.Emulation.Data;
using Roton.Emulation.Items;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Transactor(
    IParser parser,
    IState state,
    IItemEvaluator itemEvaluator)
    : ITransactor
{
    public bool Execute(ref OopContext context, ref Word instruction, bool take)
    {
        // Does the item exist?
        if (!itemEvaluator.TryEval(ref context, ref instruction, out var item))
            return false;

        // Do we have a valid amount?
        var amount = parser.ReadNumber(context.Index, ref context.Actor.Instruction);
        if (amount <= 0)
            return true;

        // Modify value if we are taking.
        if (take)
            state.OopNumber = -state.OopNumber;

        // Determine if the result will be in range.
        var pendingAmount = item!.Value + state.OopNumber;
        if ((pendingAmount & 0xFFFF) >= 0x8000)
            return true;

        // Successful transaction.
        item.Value = pendingAmount;
        return false;
    }
}