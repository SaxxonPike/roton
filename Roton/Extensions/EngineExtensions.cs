using Roton.Core;

namespace Roton.Extensions
{
    public static class EngineExtensions
    {
        public static IActor ActorAt(this IEngine engine, IXyPair location)
        {
            return engine.Actors[engine.ActorIndexAt(location)];
        }

        public static AnsiChar Draw(this IEngine engine, int x, int y)
        {
            return engine.Draw(new Location(x, y));
        }

        public static IElement ElementAt(this IEngine engine, IXyPair location)
        {
            return engine.Elements[engine.Tiles[location].Id];
        }

        public static void PlaySound(this IEngine engine, int priority, ISound sound)
        {
            engine.PlaySound(priority, sound, 0, sound.Length);
        }

        public static ITile TileAt(this IEngine engine, IXyPair location)
        {
            return engine.Tiles[location];
        }

        public static ITile TileAt(this IEngine engine, int x, int y)
        {
            return engine.Tiles[new Location(x, y)];
        }
    }
}
