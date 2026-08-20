namespace Roton.Emulation.Data;

public static class ElementListExtensions
{
    extension(IElementList elementList)
    {
        public IElement Ammo() =>
            elementList[elementList.AmmoId];

        public IElement Bullet() =>
            elementList[elementList.BulletId];

        public IElement Clockwise() =>
            elementList[elementList.ClockwiseId];

        public IElement Counter() =>
            elementList[elementList.CounterId];

        public IElement Gem() =>
            elementList[elementList.GemId];

        public IElement Invisible() =>
            elementList[elementList.InvisibleId];

        public IElement Key() =>
            elementList[elementList.KeyId];

        public IElement Player() =>
            elementList[elementList.PlayerId];

        public IElement Slime() =>
            elementList[elementList.SlimeId];

        public IElement Star() =>
            elementList[elementList.StarId];

        public IElement Torch() =>
            elementList[elementList.TorchId];
    }
}