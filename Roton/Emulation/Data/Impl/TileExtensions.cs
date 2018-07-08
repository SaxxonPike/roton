namespace Roton.Emulation.Data.Impl
{
    public static class TileExtensions
    {
        public static void CopyFrom(this ITile self, ITile tile)
        {
            self.Id = tile.Id;
            self.Color = tile.Color;
        }

        public static bool Matches(this ITile self, ITile other)
        {
            return self.Id == other.Id && self.Color == other.Color;
        }

        public static bool Matches(this ITile self, int id, int color)
        {
            return self.Id == id && self.Color == color;
        }

        public static void SetTo(this ITile self, int id, int color)
        {
            self.Id = id;
            self.Color = color;
        }

        public static void SetTo(this ITile self, IElement element)
        {
            self.Id = element.Id;
        }

        public static void SetTo(this ITile self, IElement element, int color)
        {
            SetTo(self, element);
            self.Color = color;
        }
    }
}