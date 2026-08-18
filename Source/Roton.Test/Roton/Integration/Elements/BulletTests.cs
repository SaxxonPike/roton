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
}