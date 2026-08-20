using AwesomeAssertions;
using NUnit.Framework;

namespace Roton.Test.Roton.Integration.Elements;

public class SpinningGunTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void SpinningGun_ShouldFireBulletTowardsPlayer_WhenAlignedHorizontally()
    {
        // Place the player.
        MovePlayerTo(10, 5);

        // Place the gun.
        var gunIndex = SpawnTo(5, 5, ElementList.SpinningGunId);
        var gun = Actors[gunIndex];
        gun.P1 = 9;
        gun.P2 = 9;
        gun.Cycle = 1;

        // Wait for the gun to activate.
        Step();

        // Assert.
        TileAt(6, 5).Id.Should().Be(ElementList.EmptyId,
            "bullet needs to travel one tile");
        TileAt(7, 5).Id.Should().Be(ElementList.BulletId,
            "spinning gun should shoot towards player");
    }

    [Test]
    public void SpinningGun_ShouldFireBulletTowardsPlayer_WhenAlignedVertically()
    {
        // Place the player.
        MovePlayerTo(5, 10);

        // Place the gun.
        var gunIndex = SpawnTo(5, 5, ElementList.SpinningGunId);
        var gun = Actors[gunIndex];
        gun.P1 = 9;
        gun.P2 = 9;
        gun.Cycle = 1;

        // Wait for the gun to activate.
        Step();

        // Assert.
        TileAt(5, 6).Id.Should().Be(ElementList.EmptyId,
            "bullet needs to travel one tile");
        TileAt(5, 7).Id.Should().Be(ElementList.BulletId,
            "spinning gun should shoot towards player");
    }

    [Test]
    public void SpinningGun_ShouldFireStarTowardsPlayer_WhenP2HighBitSet()
    {
        // Place the player.
        MovePlayerTo(10, 5);

        // Place the gun, setting it to shoot stars.
        var gunIndex = SpawnTo(5, 5, ElementList.SpinningGunId);
        var gun = Actors[gunIndex];
        gun.P1 = 9;
        gun.P2 = 9 | 0x80;
        gun.Cycle = 1;

        // Wait for the gun to activate.
        Step();

        // Assert.
        TileAt(6, 5).Id.Should().Be(ElementList.StarId,
            "spinning gun that shoots stars should shoot a star moving towards player");
    }
}
