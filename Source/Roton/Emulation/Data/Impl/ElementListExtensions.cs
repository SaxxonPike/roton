namespace Roton.Emulation.Data.Impl
{
    public static class ElementListExtensions
    {
        public static IElement Ammo(this IElementList elementList)
        {
            return elementList[elementList.AmmoId];
        }

        public static IElement Bullet(this IElementList elementList)
        {
            return elementList[elementList.BulletId];
        }

        public static IElement Clockwise(this IElementList elementList)
        {
            return elementList[elementList.ClockwiseId];
        }

        public static IElement Counter(this IElementList elementList)
        {
            return elementList[elementList.CounterId];
        }

        public static IElement Gem(this IElementList elementList)
        {
            return elementList[elementList.GemId];
        }

        public static IElement Invisible(this IElementList elementList)
        {
            return elementList[elementList.InvisibleId];
        }

        public static IElement Key(this IElementList elementList)
        {
            return elementList[elementList.KeyId];
        }

        public static IElement Player(this IElementList elementList)
        {
            return elementList[elementList.PlayerId];
        }

        public static IElement Slime(this IElementList elementList)
        {
            return elementList[elementList.SlimeId];
        }

        public static IElement Star(this IElementList elementList)
        {
            return elementList[elementList.StarId];
        }

        public static IElement Torch(this IElementList elementList)
        {
            return elementList[elementList.TorchId];
        }
    }
}