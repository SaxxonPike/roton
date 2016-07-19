namespace Roton
{
    public partial class Location
    {
        internal Location()
        {
            Initialize();
        }

        internal Location(int x, int y)
        {
            X = x;
            Y = y;
            Initialize();
        }

        public IXyPair Clone()
        {
            return new Location(X, Y);
        }

        public void CopyFrom(IXyPair location)
        {
            X = location.X;
            Y = location.Y;
        }

        protected virtual void Initialize()
        {
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}