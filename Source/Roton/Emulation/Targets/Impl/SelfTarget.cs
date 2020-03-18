using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets.Impl
{
    [Context(Context.Original, "SELF")]
    [Context(Context.Super, "SELF")]
    public sealed class SelfTarget : ITarget
    {
        public bool Execute(int index, ISearchContext context, string term)
        {
            if (index <= 0)
                return false;

            if (context.SearchIndex > index)
                return false;

            context.SearchIndex = index;
            return true;
        }
    }
}