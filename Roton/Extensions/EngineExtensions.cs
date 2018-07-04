using System.Linq;
using Roton.Core;

namespace Roton.Extensions
{
    public static class EngineExtensions
    {
        public static IActor ActorAt(this IActors actors, IXyPair location)
        {
            return actors
                .FirstOrDefault(actor => actor.Location.X == location.X && actor.Location.Y == location.Y);
        }

        public static AnsiChar Draw(this IEngine engine, int x, int y)
        {
            return engine.Draw(new Location(x, y));
        }

        public static void PlaySound(this IEngine engine, int priority, ISound sound)
        {
            _engine.PlaySound(priority, sound, 0, sound.Length);
        }

        public static ITile TileAt(this IGrid grid, int x, int y)
        {
            return grid[new Location(x, y)];
        }
    }
}