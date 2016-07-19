namespace Roton
{
    internal partial class Element
    {
        internal Element()
        {
            Index = -1;
        }

        public override string ToString()
        {
            return KnownName ?? string.Empty;
        }
    }
}