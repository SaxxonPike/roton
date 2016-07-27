using Roton.Emulation.Mapping;

namespace Roton.Core
{
    public interface IGrammar
    {
        void Cheat(IEngine engine, string input);
        void Execute(IOopContext context);
        bool? GetCondition(IOopContext oopContext);
        IXyPair GetDirection(IOopContext oopContext);
        IOopItem GetItem(IOopContext oopContext);
        ITile GetKind(IOopContext oopContext);
        bool GetTarget(string target, ISearchContext context);
    }
}