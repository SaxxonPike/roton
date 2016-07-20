using Roton.Core;
using Roton.Emulation.Models;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryElementBase : Element
    {
        public MemoryElementBase(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
            Act = DefaultAct;
            Draw = DefaultDraw;
            Interact = DefaultInteract;
        }

        internal abstract void CopyFrom(MemoryElementBase other);

        public static void DefaultAct(int index)
        {
        }

        public static AnsiChar DefaultDraw(IXyPair location)
        {
            return new AnsiChar(0x3F, 0x40);
        }

        public static void DefaultInteract(IXyPair location, int index, IXyPair vector)
        {
        }

        public IMemory Memory { get; private set; }

        public int Offset { get; private set; }
    }
}