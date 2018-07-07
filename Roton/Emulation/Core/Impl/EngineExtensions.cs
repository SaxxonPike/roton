using System.Linq;
using Roton.Core;
using Roton.Emulation.Execution;

namespace Roton.Extensions
{
    public static class EngineExtensions
    {
        public static IActor ActorAt(this IActors actors, IXyPair location)
        {
            return actors
                .FirstOrDefault(actor => actor.Location.X == location.X && actor.Location.Y == location.Y);
        }

        public static void Play(this ISounder sounder, int priority, ISound sound)
        {
            sounder.PlaySound(priority, sound, 0, sound.Length);
        }
    }
}