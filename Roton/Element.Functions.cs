namespace Roton
{
    public partial class Element
    {
        internal Element()
        {
            Index = -1;
        }

        public override string ToString()
        {
            return KnownName ?? "";
        }
    }
}