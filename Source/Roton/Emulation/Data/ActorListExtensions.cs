namespace Roton.Emulation.Data;

public static class ActorListExtensions
{
    public static IActor ActorAt(this IActorList list, Location location) => 
        list[list.IndexAt(location)];
}