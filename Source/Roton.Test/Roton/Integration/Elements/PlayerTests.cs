using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Integration.Elements;

public class PlayerTests(Context context) : AllContextTestFixture(context)
{
    [Test]
    public void Player_ShouldBeAbleToPickUpAmmo()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the ammo.
        PlotTo(4, 3, Elements.AmmoId);

        // Move the player into the ammo.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(Facts.DefaultAmmo + Facts.AmmoPerPickup,
            "ammo count should be correct");
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after pickup");
        Message.Should().BeEquivalentTo(Alerts.AmmoMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToPickUpGem()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the gem.
        PlotTo(4, 3, Elements.GemId);

        // Move the player into the gem.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Health.Should().Be(Facts.DefaultHealth + Facts.HealthPerGem,
            "health should be correct");
        Gems.Should().Be(Facts.DefaultGems + 1,
            "gems should be correct");
        Score.Should().Be(Facts.DefaultScore + Facts.ScorePerGem,
            "score should be correct");
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after pickup");
        Message.Should().BeEquivalentTo(Alerts.GemMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToPickUpKey_WhenKeyIsNotPossessed()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the key.
        var keyColor = RandomInt(1, 7);
        PlotTo(4, 3, Elements.KeyId, keyColor);

        // Move the player into the key.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Keys[keyColor - 1].Should().BeTrue(
            "correct key should be obtained");
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after pickup");
        Message.Should().BeEquivalentTo(Alerts.KeyPickupMessage(keyColor).Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldNotBeAbleToPickUpKey_WhenKeyIsPossessed()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the key.
        var keyColor = RandomInt(1, 7);
        PlotTo(4, 3, Elements.KeyId, keyColor);

        // Add the same color key to the player's inventory.
        Keys[keyColor - 1] = true;

        // Move the player into the key.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after pickup");
        Message.Should().BeEquivalentTo(Alerts.KeyAlreadyMessage(keyColor).Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToUseDoor_WhenKeyIsPossessed()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the door.
        var doorColor = RandomInt(1, 7);
        PlotTo(4, 3, Elements.DoorId, doorColor << 4);

        // Add the same color key to the player's inventory.
        Keys[doorColor - 1] = true;

        // Move the player into the door.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Keys[doorColor - 1].Should().BeFalse(
            "correct key should be consumed");
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after pickup");
        Message.Should().BeEquivalentTo(Alerts.DoorOpenMessage(doorColor).Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldNotBeAbleToUseDoor_WhenKeyIsNotPossessed()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the door.
        var keyColor = RandomInt(1, 7);
        PlotTo(4, 3, Elements.DoorId, keyColor << 4);

        // Move the player into the door.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(Elements.PlayerId,
            "player should be prevented from unlocking the door");
        Message.Should().BeEquivalentTo(Alerts.DoorLockedMessage(keyColor).Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToUseScroll_WhenScrollIsOneLine()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the scroll.
        var actorIndex = SpawnTo(4, 3, Elements.ScrollId);
        var message = Create<string>();
        SetActorCode(actorIndex, message);

        // Move the player into the scroll.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after pickup");
        Message.Should().BeEquivalentTo([message],
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToUseScroll_WhenScrollIsMultiLine()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the scroll.
        var underColor = RandomInt(0x00, 0xFF);
        PlotTo(4, 3, Elements.FakeId, underColor);
        var actorIndex = SpawnTo(4, 3, Elements.ScrollId);
        var message = CreateMany<string>(3).ToArray();
        SetActorCode(actorIndex, message);

        // Move the player into the scroll.
        Type(AnsiKey.Right);
        Type(AnsiKey.Enter);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(Elements.PlayerId,
            "player should not move after multi-line scroll");
        TileAt(4, 3).Id.Should().Be(Elements.FakeId,
            "scroll should leave behind under tile ID");
        TileAt(4, 3).Color.Should().Be(underColor,
            "scroll should leave behind under tile color");
        Message.Should().BeEmpty(
            "no message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToActivateBomb_WhenBombIsNotActivated()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the bomb.
        var actorIndex = SpawnTo(4, 3, Elements.BombId);
        var actor = Actors[actorIndex];

        // Move the player into the bomb.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(Elements.PlayerId,
            "player should not move for bomb activation");
        TileAt(4, 3).Id.Should().Be(Elements.BombId,
            "bomb should be present after activation");
        Message.Should().BeEquivalentTo(Alerts.BombMessage.Text,
            "correct message should be displayed");
        ((int)actor.P1).Should().Be((byte)(Facts.BombCountdownStart - 1),
            "bomb should have the maximum timer set");
    }

    [Test]
    public void Player_ShouldBeAbleToMoveBomb_WhenBombIsAlreadyActivated()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the bomb and light it.
        var actorIndex = SpawnTo(4, 3, Elements.BombId);
        var actor = Actors[actorIndex];
        actor.P1 = (byte)Facts.BombCountdownStart;

        // Move the player into the bomb.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should move for activated bomb");
        TileAt(5, 3).Id.Should().Be(Elements.BombId,
            "bomb should have moved while activated");
    }

    [Test]
    public void Player_ShouldBeAbleToUseEnergizer()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the energizer.
        PlotTo(4, 3, Elements.EnergizerId);

        // Move the player into the energizer.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        EnergyCycles.Should().Be(Facts.EnergyCyclesPerEnergizer - 1,
            "player should have correct number of energy cycles");
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after pickup");
        Message.Should().BeEquivalentTo(Alerts.EnergizerMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToInteractWithStar()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the star.
        SpawnTo(4, 3, Elements.StarId);

        // Move the player into the star.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Health.Should().Be(Facts.DefaultHealth - Facts.HealthLostPerHit,
            "player should take damage from the star");
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after interaction");
        Message.Should().BeEquivalentTo(Alerts.OuchMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToInteractWithBullet()
    {
        MovePlayerTo(3, 3);

        // Place the bullet. It cannot be spawned like normal because a
        // bullet with an assigned actor without a vector causes it to self-destruct
        // immediately.
        PlotTo(4, 3, Elements.BulletId);

        // Move the player into the bullet.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Health.Should().Be(Facts.DefaultHealth - Facts.HealthLostPerHit,
            "player should take damage from the bullet");
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after interaction");
        Message.Should().BeEquivalentTo(Alerts.OuchMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToInteractWithLava()
    {
        // Lava is only present in Super.
        if (Elements.LavaId < 0)
            Assert.Pass("Lava does not exist in this context");

        // Place the player.
        MovePlayerTo(3, 3);

        // Place the lava.
        PlotTo(4, 3, Elements.LavaId);

        // Move the player into the lava.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(Elements.PlayerId,
            "player should be in correct location after interaction");
        Message.Should().BeEquivalentTo(Alerts.WaterMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToInteractWithObject()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the object and assign it some code.
        var objectIndex = SpawnTo(4, 3, Elements.ObjectId);
        SetActorCode(objectIndex,
            ":touch",
            "#set f1"
        );

        // Move the player into the object.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Flags.Should().Contain(["F1"],
            "Object should have received touch label");
    }

    [Test]
    public void Player_ShouldBeAbleToInteractWithEnemy()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the enemy (any enemy with default behavior will do.)
        SpawnTo(4, 3, Elements.LionId);

        // Move the player into the enemy.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Health.Should().Be(Facts.DefaultHealth - Facts.HealthLostPerHit,
            "Player should take damage");
    }

    [Test]
    public void Player_ShouldBeAbleToPushPushable()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the boulder.
        PlotTo(4, 3, Elements.BoulderId);

        // Move the player into the boulder.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(4, 3).Id.Should().Be(Elements.PlayerId,
            "player should have moved into boulder space");
        TileAt(5, 3).Id.Should().Be(Elements.BoulderId,
            "boulder should have been pushed");
    }

    [Test]
    public void Player_ShouldBeAbleToShootWithSpaceBar()
    {
        // The player can shoot after having moved a direction.
        // One thing to note about the assertion: you might think that
        // the bullet should spawn one tile away. It does, but because
        // it comes later in the actor list, it is immediately processed
        // and moved one more tile in its current vector.

        // Place the player.
        MovePlayerTo(10, 10);

        // Give the player some ammo.
        Ammo = 10;

        // Face the player to the right and shoot.
        Type(AnsiKey.Right);
        Type(AnsiKey.Space);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(9,
            "ammo should have been consumed");
        TileAt(Player.Location.X + 2, 10).Id.Should().Be(Elements.BulletId,
            "bullet should have been spawned");
    }

    [Test]
    public void Player_ShouldBeAbleToShootWithShift()
    {
        // The player can shoot if pressing a direction and shift at the same time.

        // Place the player.
        MovePlayerTo(10, 10);

        // Give the player some ammo.
        Ammo = 10;

        // Face the player to the right and shoot.
        Type(AnsiKey.Right, KeyMod.Shift);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(9,
            "ammo should have been consumed");
        TileAt(Player.Location.X + 2, 10).Id.Should().Be(Elements.BulletId,
            "bullet should have been spawned");
    }

    [Test]
    public void Player_ShouldNotShoot_WhenOutOfAmmo()
    {
        // The player cannot shoot if they have no ammo, and an alert is
        // displayed the first time this happens.

        // Place the player.
        MovePlayerTo(10, 10);

        // Face the player to the right and try to shoot.
        Type(AnsiKey.Right, KeyMod.Shift);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(0,
            "player should be out of ammo");
        TileAt(Player.Location.X + 2, 10).Id.Should().Be(Elements.EmptyId,
            "bullet should not have been spawned");
        Message.Should().BeEquivalentTo(Alerts.NoAmmoMessage.Text,
            "out of ammo message should be displayed");
    }

    [Test]
    public void Player_ShouldNotShoot_WhenBoardProhibitsShooting()
    {
        // The player cannot shoot if the board's maximum bullet count is zero.

        // Place the player.
        MovePlayerTo(10, 10);

        // Give the player some ammo.
        Ammo = 10;
        
        // Prohibit shooting on this board.
        Board.MaximumShots = 0;

        // Face the player to the right and try to shoot.
        Type(AnsiKey.Right, KeyMod.Shift);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(10,
            "ammo should not have been consumed");
        TileAt(Player.Location.X + 2, 10).Id.Should().Be(Elements.EmptyId,
            "bullet should not have been spawned");
        Message.Should().BeEquivalentTo(Alerts.NoShootMessage.Text,
            "shooting not allowed message should be displayed");
    }

    [Test]
    public void Player_ShouldNotShoot_WhenBoardBulletLimitIsReached()
    {
        // The player cannot shoot if the board's maximum bullet count
        // has been reached.

        // Place the player.
        MovePlayerTo(10, 10);

        // Give the player some ammo.
        Ammo = 10;
        
        // Limit the number of bullets.
        Board.MaximumShots = 1;

        // Face the player to the right and try to shoot twice.
        Type(AnsiKey.Right, KeyMod.Shift);
        Type(AnsiKey.Right, KeyMod.Shift);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(9,
            "only one ammo should have been consumed");
        TileAt(Player.Location.X + 3, 10).Id.Should().Be(Elements.BulletId,
            "first bullet should have been spawned");
        TileAt(Player.Location.X + 2, 10).Id.Should().Be(Elements.EmptyId,
            "second bullet should not have spawned");
    }

    [Test]
    public void Player_ShouldBeAbleToShootEnemiesPointBlank()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Give the player some ammo.
        Ammo = 1;

        // Place an enemy.
        SpawnTo(11, 10, Elements.LionId);

        // Instruct the player to shoot the enemy.
        Type(AnsiKey.Right, KeyMod.Shift);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(0,
            "player should have used ammo");
        TileAt(11, 10).Id.Should().Be(Elements.EmptyId,
            "enemy should have been killed");
    }

    [Test]
    public void Player_ShouldBeAbleToShootBreakablesPointBlank()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Give the player some ammo.
        Ammo = 1;

        // Place a breakable wall.
        PlotTo(11, 10, Elements.BreakableId);

        // Instruct the player to shoot the wall.
        Type(AnsiKey.Right, KeyMod.Shift);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(0,
            "player should have used ammo");
        TileAt(11, 10).Id.Should().Be(Elements.EmptyId,
            "breakable wall should have been broken");
    }

    [Test]
    public void PlayerClone_OnTitleScreen_ShouldInteract()
    {
        // On the title screen, actor 0 element will be Monitor instead of player.
        TileAt(Player.Location).Id = Elements.MonitorId;
        State.PlayerElement = Elements.MonitorId;

        // Spawn a player clone.
        SpawnTo(5, 5, Elements.PlayerId);

        // Place an object that will receive the clone's input.
        var objectId = SpawnTo(5, 4, Elements.ObjectId);
        Actors[objectId].Cycle = 1;
        SetActorCode(objectId,
            "#end",
            ":touch",
            "#set f1"
        );

        // Make the player clone touch the object.
        Type(AnsiKey.Up);
        StepAllKeys();

        // Assert.
        Flags.AsEnumerable().Should().Contain(["F1"]);
    }
}