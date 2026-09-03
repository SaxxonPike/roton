namespace Roton.Emulation.Data;

public static class ElementListExtensions
{
    extension(IElementList elements)
    {
        public IElement Ammo() =>
            elements[elements.AmmoId];

        public IElement Bullet() =>
            elements[elements.BulletId];

        public IElement Clockwise() =>
            elements[elements.ClockwiseId];

        public IElement Counter() =>
            elements[elements.CounterId];

        public IElement Gem() =>
            elements[elements.GemId];

        public IElement Invisible() =>
            elements[elements.InvisibleId];

        public IElement Key() =>
            elements[elements.KeyId];

        public IElement Player() =>
            elements[elements.PlayerId];

        public IElement Slime() =>
            elements[elements.SlimeId];

        public IElement Star() =>
            elements[elements.StarId];

        public IElement Torch() =>
            elements[elements.TorchId];
    }
}