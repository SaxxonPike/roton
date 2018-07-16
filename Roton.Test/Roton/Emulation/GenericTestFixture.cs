using FluentAssertions;
using NUnit.Framework;
using Roton.Emulation.Data.Impl;
using Roton.Emulation.Infrastructure;
using Roton.Test.Infrastructure;

namespace Roton.Test.Roton.Emulation
{
    public class GenericTestFixture : AllContextBaseTestFixture
    {
        public GenericTestFixture(ContextEngine contextEngine) : base(contextEngine)
        {
        }

        [Test]
        public void Player_ShouldBeAbleToPickUpAmmo()
        {
            MovePlayerTo(3, 3);
            PlotAt(4, 3, ElementList.AmmoId);
            Type(EngineKeyCode.RightArrow);
            Step();
            Step();
            World.Ammo.Should().Be(Facts.DefaultAmmo + Facts.AmmoPerPickup);
        }
        
        [Test]
        public void Player_ShouldBeAbleToPickUpGem()
        {
            var health = World.Health;
            MovePlayerTo(3, 3);
            PlotAt(4, 3, ElementList.GemId);
            Type(EngineKeyCode.RightArrow);
            Step();
            Step();
            World.Health.Should().Be(Facts.DefaultHealth + Facts.HealthPerGem);
            World.Gems.Should().Be(Facts.DefaultGems + 1);
            World.Score.Should().Be(Facts.DefaultScore + Facts.ScorePerGem);
        }
    }
}