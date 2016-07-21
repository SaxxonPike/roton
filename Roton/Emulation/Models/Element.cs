using System;
using Roton.Core;

namespace Roton.Emulation.Models
{
    internal class Element : IElement
    {
        public virtual Action<int> Act { get; set; }
        public virtual string BoardEditText { get; set; }
        public virtual string EditorCategory { get; set; }
        public virtual int Character { get; set; }
        public virtual string CodeEditText { get; set; }
        public virtual int Color { get; set; }
        public virtual int Cycle { get; set; }
        public virtual bool IsDestructible { get; set; }
        public virtual Func<IXyPair, AnsiChar> Draw { get; set; }
        public virtual bool HasDrawCode { get; set; }
        public virtual bool IsEditorFloor { get; set; }
        public virtual bool IsFloor { get; set; }
        public virtual int Id { get; set; } = -1;
        public virtual Action<IXyPair, int, IXyPair> Interact { get; set; }
        public virtual int MenuKey { get; set; }
        public virtual string KnownName { get; set; }
        public virtual int MenuIndex { get; set; }
        public virtual string Name { get; set; }
        public virtual string P1EditText { get; set; }
        public virtual string P2EditText { get; set; }
        public virtual string P3EditText { get; set; }
        public virtual int Points { get; set; }
        public virtual bool IsPushable { get; set; }
        public virtual bool IsAlwaysVisible { get; set; }
        public virtual string StepEditText { get; set; }

        public override string ToString()
        {
            return KnownName ?? string.Empty;
        }
    }
}