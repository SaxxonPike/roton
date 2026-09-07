using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Damager(
    IWorld world,
    ITiles tiles,
    IHud hud,
    IState state,
    IElementList elements,
    IActorList actors,
    ISoundPlayer soundPlayer,
    ISounds sounds,
    IDeferred<ITileRemover> tileRemover,
    IFacts facts,
    IMessenger messenger,
    IAlerts alerts,
    IBoard board,
    IRadiusUpdater radiusUpdater,
    IActorManager actorManager)
    : IDamager
{
    public void Harm(int index)
    {
        var actor = actors[index];

        if (index == 0)
        {
            if (world.Health <= 0)
                return;

            world.Health -= facts.HealthLostPerHit;
            hud.UpdateStatus();
            messenger.SetMessage(facts.ShortMessageDuration, alerts.OuchMessage);
            tiles[actor.Location].Color = (tiles.ElementAt(actor.Location).Color & 0x0F) | 0x70;

            if (world.Health > 0)
            {
                world.TimePassed = 0;

                if (board.RestartOnZap)
                {
                    soundPlayer.PlaySound(4, sounds.TimeOut);
                    tileRemover.Instance.RemoveItem(actor.Location);
                    var oldLocation = actor.Location;
                    actor.Location = board.Entrance;
                    radiusUpdater.UpdateRadius(oldLocation, 0);
                    radiusUpdater.UpdateRadius(actor.Location, 0);
                    state.GamePaused = true;
                }

                soundPlayer.PlaySound(4, sounds.Ouch);
            }
            else
            {
                soundPlayer.PlaySound(5, sounds.GameOver);
            }
        }
        else
        {
            var element = tiles[actor.Location].Id;
            if (element == elements.BulletId)
                soundPlayer.PlaySound(3, sounds.BulletDie);
            else if (element != elements.ObjectId) soundPlayer.PlaySound(3, sounds.EnemyDie);

            actorManager.Free(index);
        }
    }
}