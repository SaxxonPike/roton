using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class BulletTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Bullet_ContinuesMoving_WhenTargetTileIsWalkable()
    {
        // A bullet element will continue moving in its vector direction.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, ElementList.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place some kind of walkable tile in front of the bullet.
        PlotTo(6, 5, ElementList.FakeId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "bullet should have left is previous position");
        TileAt(6, 5).Id.Should().Be(ElementList.BulletId,
            "bullet should be at its new position");
    }

    [Test]
    public void Bullet_ContinuesMoving_WhenTargetTileIsWater()
    {
        if (ElementList.WaterId < 0)
        {
            Assert.Pass("Water does not exist in this context");
            return;
        }

        // Bullets can travel over water.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, ElementList.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the terrain in front of the bullet.
        PlotTo(6, 5, ElementList.WaterId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "bullet should have left is previous position");
        TileAt(6, 5).Id.Should().Be(ElementList.BulletId,
            "bullet should be at its new position");
    }

    [Test]
    public void Bullet_ContinuesMoving_WhenTargetTileIsLava()
    {
        if (ElementList.LavaId < 0)
        {
            Assert.Pass("Lava does not exist in this context");
            return;
        }

        // Bullets can travel over water.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, ElementList.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the terrain in front of the bullet.
        PlotTo(6, 5, ElementList.LavaId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "bullet should have left is previous position");
        TileAt(6, 5).Id.Should().Be(ElementList.BulletId,
            "bullet should be at its new position");
    }

    [Test]
    public void Bullet_ReversesVector_WhenHittingRicochetDirectly()
    {
        // Bullets will reverse direction when hitting a ricochet head-on.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, ElementList.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the ricochet in front of the bullet.
        PlotTo(6, 5, ElementList.RicochetId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "bullet should have left is previous position");
        TileAt(4, 5).Id.Should().Be(ElementList.BulletId,
            "bullet should be at its new position after hitting the ricochet");
    }

    [Test]
    public void Bullet_TurnsClockwise_WhenBlockedAndRicochetIsCounterClockwise()
    {
        // Bullets will use a ricochet located counter-clockwise if hitting a non-ricochet.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, ElementList.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the wall in front of the bullet.
        PlotTo(6, 5, ElementList.SolidId);
        
        // Place a ricochet counter-clockwise.
        PlotTo(5, 4, ElementList.RicochetId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "bullet should have left is previous position");
        TileAt(5, 6).Id.Should().Be(ElementList.BulletId,
            "bullet should be at its new position after hitting the ricochet");
    }
    
    [Test]
    public void Bullet_TurnsCounterClockwise_WhenBlockedAndRicochetIsClockwise()
    {
        // Bullets will use a ricochet located clockwise if hitting a non-ricochet.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, ElementList.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the wall in front of the bullet.
        PlotTo(6, 5, ElementList.SolidId);
        
        // Place a ricochet clockwise.
        PlotTo(5, 6, ElementList.RicochetId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(ElementList.EmptyId,
            "bullet should have left is previous position");
        TileAt(5, 4).Id.Should().Be(ElementList.BulletId,
            "bullet should be at its new position after hitting the ricochet");
    }
}