using Roton.Emulation.Mapping;

namespace Roton.Emulation.Targets
{
    public class SelfTarget : ITarget
    {
        public string Name => "SELF";
        
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