using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Roton.Emulation.Core.Impl;
using Roton.Emulation.Data.Impl;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Emulation
{
    public class PlayerTestFixture : AllContextBaseTestFixture
    {
        public PlayerTestFixture(ContextEngine contextEngine) : base(contextEngine)
        {
        }

        [SetUp]
        public void __Setup()
        {
            if (ElementList.PlayerId < 0)
                Assert.Inconclusive();
        }

        [Test]
        public void Player_ShouldBeAbleToPickUpAmmo()
        {
            if (ElementList.AmmoId < 0)
                Assert.Inconclusive();

            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.AmmoId);
            Type(AnsiKey.Right);
            StepAllKeys();
            World.Ammo.Should().Be(Facts.DefaultAmmo + Facts.AmmoPerPickup, "ammo count should be correct");
            TileAt(4, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(Alerts.AmmoMessage.Text, "correct message should be displayed");
        }

        [Test]
        public void Player_ShouldBeAbleToPickUpTorch()
        {
            if (ElementList.TorchId < 0)
                Assert.Inconclusive();

            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.TorchId);
            Type(AnsiKey.Right);
            StepAllKeys();
            World.Torches.Should().Be(Facts.DefaultTorches + 1, "torch count should be correct");
            TileAt(4, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(Alerts.TorchMessage.Text, "correct message should be displayed");
        }

        [Test]
        public void Player_ShouldBeAbleToPickUpGem()
        {
            if (ElementList.GemId < 0)
                Assert.Inconclusive();

            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.GemId);
            Type(AnsiKey.Right);
            StepAllKeys();
            World.Health.Should().Be(Facts.DefaultHealth + Facts.HealthPerGem, "health should be correct");
            World.Gems.Should().Be(Facts.DefaultGems + 1, "gems should be correct");
            World.Score.Should().Be(Facts.DefaultScore + Facts.ScorePerGem, "score should be correct");
            TileAt(4, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(Alerts.GemMessage.Text, "correct message should be displayed");
        }

        [Test]
        public void Player_ShouldBeAbleToPickUpKey_WhenKeyIsNotPossessed()
        {
            if (ElementList.KeyId < 0)
                Assert.Inconclusive();

            var keyColor = RandomInt(1, 7);
            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.KeyId, keyColor);
            Type(AnsiKey.Right);
            StepAllKeys();
            World.Keys[keyColor - 1].Should().BeTrue("correct key should be obtained");
            TileAt(4, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(Alerts.KeyPickupMessage(keyColor).Text,
                "correct message should be displayed");
        }

        [Test]
        public void Player_ShouldNotBeAbleToPickUpKey_WhenKeyIsPossessed()
        {
            if (ElementList.KeyId < 0)
                Assert.Inconclusive();

            var keyColor = RandomInt(1, 7);
            World.Keys[keyColor - 1] = true;
            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.KeyId, keyColor);
            Type(AnsiKey.Right);
            StepAllKeys();
            TileAt(3, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(Alerts.KeyAlreadyMessage(keyColor).Text,
                "correct message should be displayed");
        }

        [Test]
        public void Player_ShouldBeAbleToUseDoor_WhenKeyIsPossessed()
        {
            if (ElementList.DoorId < 0)
                Assert.Inconclusive();

            var keyColor = RandomInt(1, 7);
            World.Keys[keyColor - 1] = true;
            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.DoorId, keyColor << 4);
            Type(AnsiKey.Right);
            StepAllKeys();
            World.Keys[keyColor - 1].Should().BeFalse("correct key should be consumed");
            TileAt(4, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should()
                .BeEquivalentTo(Alerts.DoorOpenMessage(keyColor).Text, "correct message should be displayed");
        }

        [Test]
        public void Player_ShouldNotBeAbleToUseDoor_WhenKeyIsNotPossessed()
        {
            if (ElementList.DoorId < 0)
                Assert.Inconclusive();

            var keyColor = RandomInt(1, 7);
            MovePlayerTo(3, 3);
            PlotTo(4, 3, ElementList.DoorId, keyColor << 4);
            Type(AnsiKey.Right);
            StepAllKeys();
            TileAt(3, 3).Id.Should().Be(ElementList.PlayerId, "player should be prevented from unlocking the door");
            Message.Should().BeEquivalentTo(Alerts.DoorLockedMessage(keyColor).Text,
                "correct message should be displayed");
        }

        [Test]
        public void Player_ShouldBeAbleToUseScroll_WhenScrollIsOneLine()
        {
            if (ElementList.ScrollId < 0)
                Assert.Inconclusive();

            MovePlayerTo(3, 3);
            var actorIndex = SpawnTo(4, 3, ElementList.ScrollId);
            var message = Create<string>();
            SetActorCode(actorIndex, message);
            Type(AnsiKey.Right);
            StepAllKeys();
            TileAt(4, 3).Id.Should().Be(ElementList.PlayerId, "player should be in correct location after pickup");
            Message.Should().BeEquivalentTo(new[] {message}, "correct message should be displayed");
        }
        
        [Test]
        public void Player_ShouldBeAbleToUseScroll_WhenScrollIsMultiLine()
        {
            if (ElementList.ScrollId < 0)
                Assert.Inconclusive();
            if (ElementList.FakeId < 0)
                Assert.Inconclusive();

            MovePlayerTo(3, 3);
            var underColor = RandomInt(0x00, 0xFF);
            PlotTo(4, 3, ElementList.FakeId, underColor);
            var actorIndex = SpawnTo(4, 3, ElementList.ScrollId);
            var message = CreateMany<string>(3).ToArray();
            SetActorCode(actorIndex, message);
            Type(AnsiKey.Right);
            Type(AnsiKey.Enter);
            StepAllKeys();
            TileAt(3, 3).Id.Should().Be(ElementList.PlayerId, "player should not move after multi-line scroll");
            TileAt(4, 3).Id.Should().Be(ElementList.FakeId, "scroll should leave behind under tile ID");
            TileAt(4, 3).Color.Should().Be(underColor, "scroll should leave behind under tile color");
            Message.Should().BeEmpty("no message should be displayed");
        }
    }
}