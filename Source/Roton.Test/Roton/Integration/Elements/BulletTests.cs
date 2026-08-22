using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class BulletTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void Bullet_ContinuesMoving_WhenTargetTileIsWalkable()
    {
        // A bullet element will continue moving in its vector direction.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, Elements.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place some kind of walkable tile in front of the bullet.
        PlotTo(6, 5, Elements.FakeId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "bullet should have left is previous position");
        TileAt(6, 5).Id.Should().Be(Elements.BulletId,
            "bullet should be at its new position");
    }

    [Test]
    public void Bullet_ReversesVector_WhenHittingRicochetDirectly()
    {
        // Bullets will reverse direction when hitting a ricochet head-on.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, Elements.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the ricochet in front of the bullet.
        PlotTo(6, 5, Elements.RicochetId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "bullet should have left is previous position");
        TileAt(4, 5).Id.Should().Be(Elements.BulletId,
            "bullet should be at its new position after hitting the ricochet");
    }

    [Test]
    public void Bullet_TurnsClockwise_WhenBlockedAndRicochetIsCounterClockwise()
    {
        // Bullets will use a ricochet located counter-clockwise if hitting a non-ricochet.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, Elements.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the wall in front of the bullet.
        PlotTo(6, 5, Elements.SolidId);

        // Place a ricochet counter-clockwise.
        PlotTo(5, 4, Elements.RicochetId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "bullet should have left is previous position");
        TileAt(5, 6).Id.Should().Be(Elements.BulletId,
            "bullet should be at its new position after hitting the ricochet");
    }

    [Test]
    public void Bullet_TurnsCounterClockwise_WhenBlockedAndRicochetIsClockwise()
    {
        // Bullets will use a ricochet located clockwise if hitting a non-ricochet.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, Elements.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the wall in front of the bullet.
        PlotTo(6, 5, Elements.SolidId);

        // Place a ricochet clockwise.
        PlotTo(5, 6, Elements.RicochetId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "bullet should have left is previous position");
        TileAt(5, 4).Id.Should().Be(Elements.BulletId,
            "bullet should be at its new position after hitting the ricochet");
    }

    [Test]
    public void Bullet_ShouldInvokeShotLabel()
    {
        // Bullets will send the "SHOT" label to objects when colliding.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, Elements.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the object in front of the bullet.
        var objectIndex = SpawnTo(6, 5, Elements.ObjectId);
        var obj = Actors[objectIndex];
        obj.Cycle = 1;
        SetActorCode(objectIndex,
            "#end",
            ":shot",
            "#set f1"
        );

        // Wait for the bullet to collide with the object.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "bullet should have been destroyed");
        Flags.AsEnumerable().Should().Contain(["F1"],
            "shot label was not invoked");
    }

    [Test]
    public void Bullet_ShouldBreakBreakableTile()
    {
        // Bullets that collide with breakable tiles will be destroyed but
        // also destroy the target tile.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, Elements.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the breakable wall in front of the bullet.
        PlotTo(6, 5, Elements.BreakableId);

        // Wait for the bullet to collide with the object.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "bullet should have been destroyed");
        TileAt(6, 5).Id.Should().Be(Elements.EmptyId,
            "breakable tile should have been destroyed");
    }
}