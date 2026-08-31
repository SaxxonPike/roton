using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
public class Spawner(
    IEngineAccessor engine,
    IState state,
    IActorList actors,
    ITiles tiles,
    IBoardUpdater boardUpdater,
    IElementList elements,
    IWorld world,
    ISoundUnit soundUnit,
    ISounds sounds)
    : ISpawner
{
    private IEngine Engine => engine.Instance;

    public void SpawnActor(Location location, Tile tile, int cycle, IActor? source)
    {
        // must reserve one actor for player, and one for messenger
        if (state.ActorCount >= actors.Capacity - 2)
            return;

        state.ActorCount++;
        var actor = actors[state.ActorCount];

        source ??= state.DefaultActor;

        actor.CopyFrom(source);
        actor.Location = location;
        actor.Cycle = cycle;
        actor.UnderTile = tiles[location];
        actor.Instruction = 0;

        if (tiles.ElementAt(actor.Location).IsEditorFloor)
        {
            var newColor = tiles[actor.Location].Color & 0x70;
            newColor |= tile.Color & 0x0F;
            tiles[actor.Location].Color = newColor;
        }
        else
        {
            tiles[actor.Location].Color = tile.Color;
        }

        tiles[actor.Location].Id = tile.Id;
        if (actor.Location.Y > 0)
            boardUpdater.UpdateBoard(actor.Location);
    }

    public bool SpawnProjectile(int elementId, Location location, Vector vector, bool enemyOwned)
    {
        var target = location + vector;
        var element = tiles.ElementAt(target);

        if (element.IsFloor || elements.IsWater(element.Id))
        {
            // The logic spawns the actor and then immediately attempts to retrieve it,
            // assuming it is the last actor in the list. But if the actor list is already
            // full, no new actors will be spawned, and the following logic affects the
            // last actor in the list anyway, regardless if it's a projectile. This is a bug
            // in all versions of the original code.

            SpawnActor(target, new Tile(elementId, elements[elementId].Color), 1, state.DefaultActor);

            var actor = actors[state.ActorCount];
            actor.P1 = unchecked((byte)(enemyOwned ? 1 : 0));
            actor.Vector = vector;
            actor.P2 = 0x64;
            return true;
        }

        if (element.Id != elements.BreakableId &&
            (!element.IsDestructible ||
             element.Id == elements.PlayerId != enemyOwned ||
             world.EnergyCycles != 0))
            return false;

        Engine.Destroy(target);
        soundUnit.PlaySound(2, sounds.BulletDie);
        return true;
    }
}