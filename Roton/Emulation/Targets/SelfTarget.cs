using Roton.Emulation.Data;
using Roton.Emulation.Data.Impl;
using Roton.Infrastructure.Impl;

namespace Roton.Emulation.Targets
{
    [Context(Context.Original, "SELF")]
    [Context(Context.Super, "SELF")]
    public sealed class SelfTarget : ITarget
    {
        public bool Execute(ISearchContext context)
        {
            if (context.SearchOffset <= 0)
                return false;

            if (context.SearchIndex > context.SearchOffset)
                return false;

            context.SearchIndex = context.SearchOffset;
            return true;
        }
    }
}