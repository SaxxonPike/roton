namespace Roton.Emulation.Data.Impl
{
    public static class ElementListExtensions
    {
        public static IElement Bullet(this IElements elements)
        {
            return elements[elements.BulletId];
        }

        public static IElement Empty(this IElements elements)
        {
            return elements[elements.EmptyId];
        }

        public static IElement Star(this IElements elements)
        {
            return elements[elements.StarId];
        }
    }
}