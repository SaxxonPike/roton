namespace Roton.Emulation.Data.Impl
{
    public abstract class Element : IElement
    {
        protected Element(int id)
        {
            Id = id;
        }

        public abstract string BoardEditText { get; set; }
        public abstract int Character { get; set; }
        public abstract string CodeEditText { get; set; }
        public abstract int Color { get; set; }
        public abstract int Cycle { get; set; }
        public abstract string EditorCategory { get; set; }
        public abstract bool HasDrawCode { get; set; }
        public int Id { get; }
        public abstract bool IsAlwaysVisible { get; set; }
        public abstract bool IsDestructible { get; set; }
        public abstract bool IsEditorFloor { get; set; }
        public abstract bool IsFloor { get; set; }
        public abstract bool IsPushable { get; set; }
        public abstract int MenuIndex { get; set; }
        public abstract int MenuKey { get; set; }
        public abstract string Name { get; set; }
        public abstract string P1EditText { get; set; }
        public abstract string P2EditText { get; set; }
        public abstract string P3EditText { get; set; }
        public abstract int Points { get; set; }
        public abstract string StepEditText { get; set; }
    }
}