using Roton.Emulation.Data;
using Roton.Emulation.Infrastructure;
using Roton.Infrastructure;

namespace Roton.Emulation.Core.Impl;

[Context(Context.Original)]
[Context(Context.Super)]
internal sealed class Navigator(
    IRandomizer randomizer,
    IActorList actors,
    IWorld world)
    : INavigator
{
    public Vector Rnd()
    {
        var result = new Vector
        {
            X = randomizer.GetNext(3) - 1
        };

        result.Y = result.X == 0 ? (randomizer.GetNext(2) << 1) - 1 : 0;
        return result;
    }

    public Vector RndP(Vector vector) =>
        randomizer.GetNext(2) == 0
            ? vector.Clockwise()
            : vector.CounterClockwise();

    public Vector Seek(Location location)
    {
        var result = new Vector();
        if (randomizer.GetNext(2) == 0 || actors.Player.Location.Y == location.Y)
            result.X = (actors.Player.Location.X - location.X).Polarity();

        if (result.X == 0) result.Y = (actors.Player.Location.Y - location.Y).Polarity();

        if (world.EnergyCycles > 0)
            result = -result;

        return result;
    }

}