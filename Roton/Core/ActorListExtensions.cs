namespace Roton.Core
{
    public static class ActorListExtensions
    {
        public static int ActorIndexAt(this IActors actors, IXyPair location)
        {
            var index = 0;
            foreach (var actor in actors)
            {
                if (actor.Location.X == location.X && actor.Location.Y == location.Y)
                    return index;
                index++;
            }
            return -1;
        }
    }
}