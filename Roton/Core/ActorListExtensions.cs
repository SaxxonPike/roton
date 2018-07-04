namespace Roton.Core
{
    public static class ActorListExtensions
    {
        public static IActor GetPlayer(this IActorList actorList)
        {
            return actorList[0];
        }
    }
}