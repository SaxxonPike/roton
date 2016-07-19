namespace Roton
{
    public partial class Vector
    {
        public virtual int X { get; set; }
        public virtual int Y { get; set; }

        public static Vector East => new Vector(1, 0);

        public static Vector North => new Vector(0, -1);

        public static Vector South => new Vector(0, 1);

        public static Vector West => new Vector(-1, 0);
    }
}