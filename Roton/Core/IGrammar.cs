namespace Roton.Core
{
    public interface IGrammar
    {
        void Cheat(ICore core, string input);
        void Execute(IOopContext oopContext);
        bool? GetCondition(IOopContext oopContext);
        IXyPair GetDirection(IOopContext oopContext);
        IOopItem GetItem(IOopContext oopContext);
        ITile GetKind(IOopContext oopContext);
    }
}