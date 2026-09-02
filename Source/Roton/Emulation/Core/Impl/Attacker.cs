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
    ISoundUnit soundUnit,
    ISounds sounds,
    ITileRemover tileRemover,
    IFacts facts,
    IMessenger messenger,
    IAlerts alerts,
    IBoard board,
    IRadiusUpdater radiusUpdater,
    IActorRemover actorRemover)
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
            Harm(index);
        }

        if (index > 0 && index <= state.ActIndex) state.ActIndex--;

        if (tiles[location].Id == elements.PlayerId && world.EnergyCycles > 0)
        {
            world.Score += tiles.ElementAt(actors[index].Location).Points;
            hud.UpdateStatus();
        }
        else
        {
            Destroy(location);
            soundUnit.PlaySound(2, sounds.EnemySuicide);
        }
    }

    public void Destroy(Location location)
    {
        var index = actors.ActorIndexAt(location);
        if (index == -1)
            tileRemover.RemoveItem(location);
        else
            Harm(index);
    }

    public void Harm(int index)
    {
        var actor = actors[index];
        if (index == 0)
        {
            if (world.Health > 0)
            {
                world.Health -= facts.HealthLostPerHit;
                hud.UpdateStatus();
                messenger.SetMessage(facts.ShortMessageDuration, alerts.OuchMessage);
                tiles[actor.Location].Color = (tiles.ElementAt(actor.Location).Color & 0x0F) | 0x70;

                if (world.Health > 0)
                {
                    world.TimePassed = 0;
                    if (board.RestartOnZap)
                    {
                        soundUnit.PlaySound(4, sounds.TimeOut);
                        tileRemover.RemoveItem(actor.Location);
                        var oldLocation = actor.Location;
                        actor.Location = board.Entrance;
                        radiusUpdater.UpdateRadius(oldLocation, 0);
                        radiusUpdater.UpdateRadius(actor.Location, 0);
                        state.GamePaused = true;
                    }

                    soundUnit.PlaySound(4, sounds.Ouch);
                }
                else
                {
                    soundUnit.PlaySound(5, sounds.GameOver);
                }
            }
        }
        else
        {
            var element = tiles[actor.Location].Id;
            if (element == elements.BulletId)
                soundUnit.PlaySound(3, sounds.BulletDie);
            else if (element != elements.ObjectId) soundUnit.PlaySound(3, sounds.EnemyDie);

            actorRemover.RemoveActor(index);
        }
    }
}