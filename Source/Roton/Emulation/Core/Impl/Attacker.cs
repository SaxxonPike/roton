using Roton.Emulation.Data;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Attacker(
    IWorld world,
    ITiles tiles,
    IHud hud,
    IState state,
    IElementList elements,
    IActorList actors,
    ISoundPlayer soundPlayer,
    ISounds sounds,
    IDamager damager,
    IDestroyer destroyer)
    : IAttacker
{
    public void Attack(int index, Location location)
    {
        if (index == 0 && world.EnergyCycles > 0)
        {
            world.Score += tiles.ElementAt(location).Points;
            hud.UpdateStatus();
        }
        else
        {
            damager.Harm(index);
        }

        if (index > 0 && index <= state.ActIndex) 
            state.ActIndex--;

        if (tiles[location].Id == elements.PlayerId && world.EnergyCycles > 0)
        {
            world.Score += tiles.ElementAt(actors[index].Location).Points;
            hud.UpdateStatus();
        }
        else
        {
            destroyer.Destroy(location);
            soundPlayer.PlaySound(2, sounds.EnemySuicide);
        }
    }
}