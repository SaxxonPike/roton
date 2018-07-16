using FluentAssertions;
using NUnit.Framework;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Emulation
{
    public class PlayerTestFixture : AllContextBaseTestFixture
    {
        public PlayerTestFixture(ContextEngine contextEngine) : base(contextEngine)
        {
        }

        [Test]
        public void Player_ShouldBeAbleToPickUpAmmo()
        {
            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.AmmoId);
            Type(EngineKeyCode.RightArrow);
            Step();
            Step();
            World.Ammo.Should().Be(Facts.DefaultAmmo + Facts.AmmoPerPickup, "ammo count should be correct");
            TileAt(4, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(Alerts.AmmoMessage.Text, "correct message should be displayed");
        }
        
        [Test]
        public void Player_ShouldBeAbleToPickUpGem()
        {
            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.GemId);
            Type(EngineKeyCode.RightArrow);
            Step();
            Step();
            World.Health.Should().Be(Facts.DefaultHealth + Facts.HealthPerGem, "health should be correct");
            World.Gems.Should().Be(Facts.DefaultGems + 1, "gems should be correct");
            World.Score.Should().Be(Facts.DefaultScore + Facts.ScorePerGem, "score should be correct");
            TileAt(4, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(Alerts.GemMessage.Text, "correct message should be displayed");
        }
        
        [Test]
        public void Player_ShouldBeAbleToPickUpKey_WhenKeyIsNotPossessed()
        {
            var keyColor = RandomInt(1, 7);
            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.KeyId, keyColor);
            Type(EngineKeyCode.RightArrow);
            Step();
            Step();
            World.Keys[keyColor - 1].Should().BeTrue("correct key should be obtained");
            TileAt(4, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(Alerts.KeyPickupMessage(keyColor).Text, "correct message should be displayed");
        }
        
        [Test]
        public void Player_ShouldNotBeAbleToPickUpKey_WhenKeyIsPossessed()
        {
            var keyColor = RandomInt(1, 7);
            World.Keys[keyColor - 1] = true;
            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.KeyId, keyColor);
            Type(EngineKeyCode.RightArrow);
            Step();
            Step();
            TileAt(3, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(Alerts.KeyAlreadyMessage(keyColor).Text, "correct message should be displayed");
        }
        
        [Test]
        public void Player_ShouldBeAbleToUseDoor_WhenKeyIsPossessed()
        {
            var keyColor = RandomInt(1, 7);
            World.Keys[keyColor - 1] = true;
            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.DoorId, keyColor << 4);
            Type(EngineKeyCode.RightArrow);
            Step();
            Step();
            World.Keys[keyColor - 1].Should().BeFalse("correct key should be consumed");
            TileAt(4, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(Alerts.DoorOpenMessage(keyColor).Text, "correct message should be displayed");
        }
        
        [Test]
        public void Player_ShouldNotBeAbleToUseDoor_WhenKeyIsNotPossessed()
        {
            var keyColor = RandomInt(1, 7);
            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.DoorId, keyColor << 4);
            Type(EngineKeyCode.RightArrow);
            Step();
            Step();
            TileAt(3, 3).Id.Should().Be(ElementList.PlayerId, "player should be prevented from unlocking the door");
            Message.Should().BeEquivalentTo(Alerts.DoorLockedMessage(keyColor).Text, "correct message should be displayed");
        }
    }
}