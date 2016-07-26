using System;
using Roton.Core;
using Roton.Emulation.Behavior;

namespace Roton.Emulation.Mapping
{
    internal abstract class Element : IElement
    {
        protected Element(IMemory memory, int offset, IBehavior behavior)
        {
            Memory = memory;
            Offset = offset;
            Behavior = behavior;
        }

        private IBehavior Behavior { get; }

        protected IMemory Memory { get; private set; }

        protected int Offset { get; private set; }
        public virtual Action<IEngine, int> Act => Behavior.Act;
        public virtual string BoardEditText { get; set; }
        public virtual int Character { get; set; }
        public virtual string CodeEditText { get; set; }
        public virtual int Color { get; set; }
        public virtual int Cycle { get; set; }
        public virtual Func<IEngine, IXyPair, AnsiChar> Draw => Behavior.Draw;
        public virtual string EditorCategory { get; set; }
        public virtual bool HasDrawCode { get; set; }
        public virtual int Id { get; set; } = -1;
        public virtual Action<IEngine, IXyPair, int, IXyPair> Interact => Behavior.Interact;
        public virtual bool IsAlwaysVisible { get; set; }
        public virtual bool IsDestructible { get; set; }
        public virtual bool IsEditorFloor { get; set; }
        public virtual bool IsFloor { get; set; }
        public virtual bool IsPushable { get; set; }
        public virtual int MenuIndex { get; set; }
        public virtual int MenuKey { get; set; }
        public virtual string Name { get; set; }
        public virtual string P1EditText { get; set; }
        public virtual string P2EditText { get; set; }
        public virtual string P3EditText { get; set; }
        public virtual int Points { get; set; }
        public virtual string StepEditText { get; set; }

        public override string ToString()
        {
            return Behavior.KnownName ?? string.Empty;
        }
    }
}