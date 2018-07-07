using Roton.Core;

namespace Roton.Emulation.Items
{
    public interface IItem
    {
        string Name { get; }
        IOopItem Execute(IOopContext oopContext);
    }
}