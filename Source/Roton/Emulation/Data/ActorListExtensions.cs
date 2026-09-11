namespace Roton.Emulation.Data;

internal static class ActorListExtensions
{
    public static IActor ActorAt(this IActorList list, Location location) => 
        list[list.IndexAt(location)];
}