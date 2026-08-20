using System.Linq;
using AwesomeAssertions;
using NUnit.Framework;
using Roton.Emulation.Core.Impl;

namespace Roton.Test.Roton.Integration.Elements;

public class PlayerTests(Context context) : ElementTestFixture(context)
{
    [Test]
    public void Player_ShouldBeAbleToPickUpAmmo()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the ammo.
        PlotTo(4, 3, ElementList.AmmoId);

        // Move the player into the ammo.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(Facts.DefaultAmmo + Facts.AmmoPerPickup,
            "ammo count should be correct");
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
            "player should be in correct location after pickup");
        Message.Should().BeEquivalentTo(Alerts.AmmoMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToPickUpTorch()
    {
        if (ElementList.TorchId < 0)
            Assert.Pass("Torch does not exist in this context");

        // Place the player.
        MovePlayerTo(3, 3);

        // Place the torch.
        PlotTo(4, 3, ElementList.TorchId);

        // Move the player into the torch.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Torches.Should().Be(Facts.DefaultTorches + 1,
            "torch count should be correct");
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
            "player should be in correct location after pickup");
        Message.Should().BeEquivalentTo(Alerts.TorchMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToPickUpGem()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the gem.
        PlotTo(4, 3, ElementList.GemId);

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
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
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
        PlotTo(4, 3, ElementList.KeyId, keyColor);

        // Move the player into the key.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Keys[keyColor - 1].Should().BeTrue(
            "correct key should be obtained");
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
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
        PlotTo(4, 3, ElementList.KeyId, keyColor);

        // Add the same color key to the player's inventory.
        Keys[keyColor - 1] = true;

        // Move the player into the key.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(ElementList.PlayerId,
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
        PlotTo(4, 3, ElementList.DoorId, doorColor << 4);

        // Add the same color key to the player's inventory.
        Keys[doorColor - 1] = true;

        // Move the player into the door.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Keys[doorColor - 1].Should().BeFalse(
            "correct key should be consumed");
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
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
        PlotTo(4, 3, ElementList.DoorId, keyColor << 4);

        // Move the player into the door.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(ElementList.PlayerId,
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
        var actorIndex = SpawnTo(4, 3, ElementList.ScrollId);
        var message = Create<string>();
        SetActorCode(actorIndex, message);

        // Move the player into the scroll.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
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
        PlotTo(4, 3, ElementList.FakeId, underColor);
        var actorIndex = SpawnTo(4, 3, ElementList.ScrollId);
        var message = CreateMany<string>(3).ToArray();
        SetActorCode(actorIndex, message);

        // Move the player into the scroll.
        Type(AnsiKey.Right);
        Type(AnsiKey.Enter);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(ElementList.PlayerId,
            "player should not move after multi-line scroll");
        TileAt(4, 3).Id.Should().Be(ElementList.FakeId,
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
        var actorIndex = SpawnTo(4, 3, ElementList.BombId);
        var actor = Actors[actorIndex];

        // Move the player into the bomb.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(ElementList.PlayerId,
            "player should not move for bomb activation");
        TileAt(4, 3).Id.Should().Be(ElementList.BombId,
            "bomb should be present after activation");
        Message.Should().BeEquivalentTo(Alerts.BombMessage.Text,
            "correct message should be displayed");
        ((int)actor.P1).Should().Be((byte)(Engine.Facts.BombCountdownStart - 1),
            "bomb should have the maximum timer set");
    }

    [Test]
    public void Player_ShouldBeAbleToMoveBomb_WhenBombIsAlreadyActivated()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the bomb and light it.
        var actorIndex = SpawnTo(4, 3, ElementList.BombId);
        var actor = Actors[actorIndex];
        actor.P1 = (byte)Engine.Facts.BombCountdownStart;

        // Move the player into the bomb.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
            "player should move for activated bomb");
        TileAt(5, 3).Id.Should().Be(ElementList.BombId,
            "bomb should have moved while activated");
    }

    [Test]
    public void Player_ShouldBeAbleToUseEnergizer()
    {
        // Place the player.
        MovePlayerTo(3, 3);

        // Place the energizer.
        PlotTo(4, 3, ElementList.EnergizerId);

        // Move the player into the energizer.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        EnergyCycles.Should().Be(Facts.EnergyCyclesPerEnergizer - 1,
            "player should have correct number of energy cycles");
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
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
        SpawnTo(4, 3, ElementList.StarId);

        // Move the player into the star.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Health.Should().Be(Facts.DefaultHealth - Facts.HealthLostPerHit,
            "player should take damage from the star");
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
            "player should be in correct location after interaction");
        Message.Should().BeEquivalentTo(Alerts.OuchMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToInteractWithBullet()
    {
        // Place the player.
        if (ElementList.BulletId < 0)
            Assert.Pass("Star does not exist in this context");

        MovePlayerTo(3, 3);

        // Place the bullet. It cannot be spawned like normal because a
        // bullet with an assigned actor without a vector causes it to self-destruct
        // immediately.
        PlotTo(4, 3, ElementList.BulletId);

        // Move the player into the bullet.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        Health.Should().Be(Facts.DefaultHealth - Facts.HealthLostPerHit,
            "player should take damage from the bullet");
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
            "player should be in correct location after interaction");
        Message.Should().BeEquivalentTo(Alerts.OuchMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToInteractWithWater()
    {
        // Water is only present in Original.
        if (ElementList.WaterId < 0)
            Assert.Pass("Lava does not exist in this context");

        // Place the player.
        MovePlayerTo(3, 3);

        // Place the water.
        PlotTo(4, 3, ElementList.WaterId);

        // Move the player into the water.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(ElementList.PlayerId,
            "player should be in correct location after interaction");
        Message.Should().BeEquivalentTo(Alerts.WaterMessage.Text,
            "correct message should be displayed");
    }

    [Test]
    public void Player_ShouldBeAbleToInteractWithLava()
    {
        // Lava is only present in Super.
        if (ElementList.LavaId < 0)
            Assert.Pass("Lava does not exist in this context");

        // Place the player.
        MovePlayerTo(3, 3);

        // Place the lava.
        PlotTo(4, 3, ElementList.LavaId);

        // Move the player into the lava.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(3, 3).Id.Should().Be(ElementList.PlayerId,
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
        var objectIndex = SpawnTo(4, 3, ElementList.ObjectId);
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
        SpawnTo(4, 3, ElementList.LionId);

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
        PlotTo(4, 3, ElementList.BoulderId);

        // Move the player into the boulder.
        Type(AnsiKey.Right);
        StepAllKeys();

        // Assert.
        TileAt(4, 3).Id.Should().Be(ElementList.PlayerId,
            "player should have moved into boulder space");
        TileAt(5, 3).Id.Should().Be(ElementList.BoulderId,
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
        TileAt(Player.Location.X + 2, 10).Id.Should().Be(ElementList.BulletId,
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
        TileAt(Player.Location.X + 2, 10).Id.Should().Be(ElementList.BulletId,
            "bullet should have been spawned");
    }

    [Test]
    public void Player_ShouldBeAbleToShootEnemiesPointBlank()
    {
        // Place the player.
        MovePlayerTo(10, 10);

        // Give the player some ammo.
        Ammo = 1;

        // Place an enemy.
        SpawnTo(11, 10, ElementList.LionId);

        // Instruct the player to shoot the enemy.
        Type(AnsiKey.Right, KeyMod.Shift);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(0,
            "player should have used ammo");
        TileAt(11, 10).Id.Should().Be(ElementList.EmptyId,
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
        PlotTo(11, 10, ElementList.BreakableId);

        // Instruct the player to shoot the wall.
        Type(AnsiKey.Right, KeyMod.Shift);
        StepAllKeys();

        // Assert.
        Ammo.Should().Be(0,
            "player should have used ammo");
        TileAt(11, 10).Id.Should().Be(ElementList.EmptyId,
            "breakable wall should have been broken");
    }

    [Test]
    public void PlayerClone_OnTitleScreen_ShouldInteract()
    {
        // On the title screen, actor 0 element will be Monitor instead of player.
        TileAt(Player.Location).Id = ElementList.MonitorId;
        State.PlayerElement = ElementList.MonitorId;
        
        // Spawn a player clone.
        SpawnTo(5, 5, ElementList.PlayerId);
        
        // Place an object that will receive the clone's input.
        var objectId = SpawnTo(5, 4, ElementList.ObjectId);
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