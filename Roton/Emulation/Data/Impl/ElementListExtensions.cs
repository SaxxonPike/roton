namespace Roton.Emulation.Data.Impl
{
    public static class ElementListExtensions
    {
        public static IElement Bullet(this IElementList elementList)
        {
            return elementList[elementList.BulletId];
        }

        public static IElement Empty(this IElementList elementList)
        {
            return elementList[elementList.EmptyId];
        }

        public static IElement Star(this IElementList elementList)
        {
            return elementList[elementList.StarId];
        }
    }
}