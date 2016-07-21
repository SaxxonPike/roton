using System;
using Roton.Core;
using Roton.Emulation.Models;

namespace Roton.Emulation.Mapping
{
    internal abstract class MemoryElementBase : Element
    {
        protected MemoryElementBase(IMemory memory, int offset)
        {
            Memory = memory;
            Offset = offset;
        }

        public override Action<int> Act { get; set; } = DefaultAct;
        public override Func<IXyPair, AnsiChar> Draw { get; set; } = DefaultDraw;
        public override Action<IXyPair, int, IXyPair> Interact { get; set; } = DefaultInteract;

        private static void DefaultAct(int index)
        {
        }

        private static AnsiChar DefaultDraw(IXyPair location)
        {
            return new AnsiChar(0x3F, 0x40);
        }

        private static void DefaultInteract(IXyPair location, int index, IXyPair vector)
        {
        }

        protected IMemory Memory { get; private set; }

        protected int Offset { get; private set; }
    }
}