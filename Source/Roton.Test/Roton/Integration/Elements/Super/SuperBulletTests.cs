using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Data;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements.Super;

public class SuperBulletTests : SuperContextTestFixture
{
    [Test]
    public void Bullet_ContinuesMoving_WhenTargetTileIsLava()
    {
        if (Elements.LavaId < 0)
        {
            Assert.Pass("Lava does not exist in this context");
            return;
        }

        // Bullets can travel over water.

        // Place the bullet and assign it a vector.
        var bulletIndex = SpawnTo(5, 5, Elements.BulletId);
        var bullet = Actors[bulletIndex];
        bullet.Vector = Vector.East;
        bullet.Cycle = 1;

        // Place the terrain in front of the bullet.
        PlotTo(6, 5, Elements.LavaId);

        // Wait for the bullet to move.
        Step();

        // Assert.
        TileAt(5, 5).Id.Should().Be(Elements.EmptyId,
            "bullet should have left is previous position");
        TileAt(6, 5).Id.Should().Be(Elements.BulletId,
            "bullet should be at its new position");
    }
}