namespace Roton.Emulation.Execution
{
    internal class CodeSearchInfo
    {
        public CodeSearchInfo(string label)
        {
            Index = 0;
            Label = label;
            Offset = 0;
        }

        public virtual int Index { get; set; }

        public virtual string Label { get; set; }

        public virtual int Offset { get; set; }
    }
}